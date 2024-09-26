using Application.Extensions;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using Application.Features.Appointments.Constants;
using Application.Services.CalendarService;
using Domain.Entities;
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
            Title = AppointmentsMessages.Appointment,
            Description = $"{appointment.Client?.FullName}\n{appointment.Client?.Contact}",
            Color = appointmentStatusProps.ColorId,
            Status = "confirmed",
            StartDate = appointment.StartDate,
            EndDate = appointment.EndDate
        };
        return calendarEvent;
    }

    public async Task<Appointment> AddCalendarEvent(Appointment appointment, CancellationToken cancellationToken)
    {
        var calendarEvent = CreateCalendarEvent(appointment);
        calendarEvent = await _calendarService.AddEvent(calendarEvent, cancellationToken);
        appointment.CalendarEventId = calendarEvent.Id;
        return appointment;
    }

    public async Task<Appointment> UpdateCalendarEvent(Appointment appointment, CancellationToken cancellationToken)
    {
        var calendarEvent = CreateCalendarEvent(appointment);
        calendarEvent.Id = appointment.CalendarEventId;
        await _calendarService.UpdateEvent(calendarEvent, cancellationToken);
        return appointment;
    }

    public async Task DeleteCalendarEvent(Appointment appointment, CancellationToken cancellationToken)
    {
        await _calendarService.DeleteEvent(appointment.CalendarEventId, cancellationToken);
    }

    public async Task UpdateCalendarEventColor(Appointment appointment, CancellationToken cancellationToken)
    {
        var appointmentStatusProps = appointment.Status.GetProps();
        var color = appointmentStatusProps.ColorId;
        var calendarEvent =
            await _calendarService.UpdateEventColor(appointment.CalendarEventId, color, cancellationToken);
        if (!calendarEvent.Color.Equals(color))
        {
            throw new BusinessException(AppointmentsMessages.FailedColorUpdate);
        }
    }

    public async Task<IEnumerable<CalendarEvent>> GetUpdatedEvents()
    {
        return await _calendarService.GetUpdatedEvents();
    }
    
}