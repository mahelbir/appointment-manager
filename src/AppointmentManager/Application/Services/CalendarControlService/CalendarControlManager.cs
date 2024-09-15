using System.Text.RegularExpressions;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using Application.Features.Appointments.Constants;
using Application.Features.Appointments.Rules;
using Application.Services.AppointmentService;
using Application.Services.CalendarService;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;

namespace Application.Services.CalendarControlService;

public class CalendarControlManager : ICalendarControlService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IAppointmentService _appointmentService;
    private readonly ICalendarService _calendarService;
    private readonly AppointmentBusinessRules _appointmentBusinessRules;

    public CalendarControlManager(IAppointmentRepository appointmentRepository, IAppointmentService appointmentService,
        ICalendarService calendarService, AppointmentBusinessRules appointmentBusinessRules)
    {
        _appointmentRepository = appointmentRepository;
        _appointmentService = appointmentService;
        _calendarService = calendarService;
        _appointmentBusinessRules = appointmentBusinessRules;
    }

    private CalendarEvent CreateCalendarEvent(Appointment appointment)
    {
        var appointmentStatusProps = _appointmentService.GetAppointmentStatus(appointment.Status);
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

    public async Task ValidateCalendarToken(string token)
    {
        await _appointmentBusinessRules.CantEmpty(token);
        await _appointmentBusinessRules.TokenShouldMatch(token, _calendarService.CalendarToken);
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
        var appointmentStatusProps = _appointmentService.GetAppointmentStatus(appointment.Status);
        var color = appointmentStatusProps.ColorId;
        var calendarEvent =
            await _calendarService.UpdateEventColor(appointment.CalendarEventId, color, cancellationToken);
        if (!calendarEvent.Color.Equals(color))
        {
            throw new BusinessException(AppointmentsMessages.FailedColorUpdate);
        }
    }

    public async Task ReceiveCalendarEvents(CancellationToken cancellationToken)
    {
        var calendarEvents = await _calendarService.GetUpdatedEvents();
        foreach (var calendarEvent in calendarEvents)
        {
            try
            {
                var appointment = await _appointmentService.GetByCalendarEventId(calendarEvent.Id);
                if (appointment != null)
                {
                    await UpdateReceivedCalendarEvent(appointment, calendarEvent, cancellationToken);
                }
                else
                {
                    await AddReceivedCalendarEvent(calendarEvent, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    private async Task<Appointment?> AddReceivedCalendarEvent(CalendarEvent calendarEvent,
        CancellationToken cancellationToken)
    {
        if (calendarEvent.Status == "cancelled") return null;

        await _appointmentBusinessRules.CantOverlap(calendarEvent.StartDate, calendarEvent.EndDate);

        var appointmentStatusProps = _appointmentService.GetAppointmentStatuses().Values
            .First(s => s.ColorId == calendarEvent.Color);
        var client = ParseClientFromDescription(calendarEvent.Description);
        client.CreatedDate = DateTime.UtcNow;
        var appointment = new Appointment
        {
            StartDate = calendarEvent.StartDate,
            EndDate = calendarEvent.EndDate,
            Status = appointmentStatusProps.Status,
            CalendarEventId = calendarEvent.Id,
            Client = client
        };

        return await _appointmentRepository.AddAsync(appointment, cancellationToken);
    }

    private async Task<Appointment> UpdateReceivedCalendarEvent(Appointment appointment, CalendarEvent calendarEvent,
        CancellationToken cancellationToken)
    {
        if (calendarEvent.Status == "cancelled")
        {
            return await SetCanceled();
        }

        var appointmentStatusProps = _appointmentService.GetAppointmentStatuses().Values
            .FirstOrDefault(s => s.ColorId == calendarEvent.Color);

        if (appointmentStatusProps == null || appointmentStatusProps.Status == AppointmentStatus.Cancelled)
        {
            return await SetCanceled();
        }
        
        await _appointmentBusinessRules.CantOverlap(calendarEvent.StartDate, calendarEvent.EndDate, appointment.Id);

        appointment.StartDate = calendarEvent.StartDate;
        appointment.EndDate = calendarEvent.EndDate;
        appointment.Status = appointmentStatusProps.Status;

        if (appointment.Client != null)
        {
            var client = ParseClientFromDescription(calendarEvent.Description);
            appointment.Client.FirstName = client.FirstName;
            appointment.Client.LastName = client.LastName;
            appointment.Client.Contact = client.Contact;
            appointment.Client.UpdatedDate = DateTime.UtcNow;
        }

        await _appointmentRepository.UpdateAsync(appointment, cancellationToken);

        return appointment;

        async Task<Appointment> SetCanceled()
        {
            if (appointment.Status != AppointmentStatus.Cancelled)
            {
                appointment.Status = AppointmentStatus.Cancelled;
                await _appointmentRepository.UpdateAsync(appointment, cancellationToken);
            }

            return appointment;
        }
    }

    private static Client ParseClientFromDescription(string description)
    {
        description = description.Replace("<br>", "\n");
        var lines = description
            .Split('\n')
            .Select(s => Regex.Replace(s, "<.*?>", String.Empty).Trim())
            .ToArray();

        var nameField = lines[0];
        var names = nameField.Contains(' ') ? nameField.Split(' ', 2) : [nameField, ""];
        var contact = lines.Length > 1 ? lines[1] : "";

        return new Client
        {
            FirstName = names[0],
            LastName = names[1],
            Contact = contact
        };
    }
}