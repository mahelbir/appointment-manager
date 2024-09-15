using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using Application.Features.Appointments.Constants;
using Application.Services.AppointmentService;
using Application.Services.CalendarService;
using Domain.Entities;
using Domain.Models;

namespace Application.Services.CalendarControlService;

public class CalendarControlManager : ICalendarControlService
{
    private readonly IAppointmentService _appointmentService;
    private readonly ICalendarService _calendarService;

    public CalendarControlManager(IAppointmentService appointmentService, ICalendarService calendarService)
    {
        _appointmentService = appointmentService;
        _calendarService = calendarService;
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
}