using Application.Extensions;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using Application.Features.Appointments.Constants;
using Application.Services.CalendarService;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;

namespace Application.Services.CalendarControlService;

public class CalendarControlManager : ICalendarControlService
{
    private readonly ICalendarService _calendarService;

    public CalendarControlManager(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    public async Task ValidateCalendarToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new BusinessException("Token gerekli");
        }

        if (token != _calendarService.CalendarToken)
        {
            throw new BusinessException("Token eşleşmiyor");
        }
    }

    private CalendarEvent CreateCalendarEvent(Appointment appointment)
    {
        var appointmentStatusProps = appointment.Status.GetProps();
        var calendarEvent = new CalendarEvent
        {
            Id = appointment.CalendarEventId,
            Title = AppointmentsMessages.Appointment,
            Description = $"{appointment.Client?.FullName}\n{appointment.Client?.Contact}",
            Color = appointmentStatusProps.ColorId,
            Status = "confirmed",
            StartDate = appointment.StartDate,
            EndDate = appointment.EndDate
        };
        return calendarEvent;
    }

    public async Task<CalendarEvent> AddCalendarEvent(Appointment appointment, CancellationToken cancellationToken)
    {
        var calendarEvent = CreateCalendarEvent(appointment);
        calendarEvent = await _calendarService.AddEvent(calendarEvent, cancellationToken);
        appointment.CalendarEventId = calendarEvent.Id;
        return calendarEvent;
    }

    public async Task DeleteCalendarEvent(Appointment appointment, CancellationToken cancellationToken)
    {
        await _calendarService.DeleteEvent(appointment.CalendarEventId, cancellationToken);
    }

    public async Task<CalendarEvent> UpdateCalendarEvent(Appointment appointment, CancellationToken cancellationToken)
    {
        var calendarEvent = CreateCalendarEvent(appointment);
        await _calendarService.UpdateEvent(calendarEvent, cancellationToken);
        return calendarEvent;
    }

    public async Task<CalendarEvent> UpdateCalendarEventColor(Appointment appointment,
        CancellationToken cancellationToken)
    {
        var appointmentStatusProps = appointment.Status.GetProps();
        var color = appointmentStatusProps.ColorId;
        var calendarEvent =
            await _calendarService.UpdateEventColor(appointment.CalendarEventId, color, cancellationToken);
        if (!calendarEvent.Color.Equals(color))
        {
            throw new BusinessException(AppointmentsMessages.FailedColorUpdate);
        }

        return calendarEvent;
    }

    public async Task<IEnumerable<CalendarEvent>> UpdateCalendarEventsClient(IEnumerable<Appointment> appointments,
        Client client, CancellationToken cancellationToken)
    {
        appointments = appointments.ToList();
        List<CalendarEvent> calendarEvents = [];
        var eventTasks = new List<Task<CalendarEvent>>();

        foreach (var appointment in appointments)
        {
            if (appointment.Status == AppointmentStatus.Cancelled) continue;
            appointment.Client = client;
            eventTasks.Add(Task.Run(async () =>
            {
                var calendarEvent = CreateCalendarEvent(appointment);
                try
                {
                    await _calendarService.UpdateEvent(calendarEvent, cancellationToken);
                }
                catch (Exception e)
                {
                    calendarEvent.Description = "";
                    Console.WriteLine($"Failed to update calendar for appointment {appointment.Id}: {e.Message}");
                }

                calendarEvents.Add(calendarEvent);
                return calendarEvent;
            }, cancellationToken));
        }

        await Task.WhenAll(eventTasks);

        return calendarEvents;
    }

    public async Task<IEnumerable<CalendarEvent>> GetUpdatedEvents()
    {
        return await _calendarService.GetUpdatedEvents();
    }
    
}