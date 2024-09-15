using Application.Features.Appointments.Constants;
using Application.Services.CalendarService;
using Application.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;

namespace Application.Services.AppointmentService;

public class AppointmentManager : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICalendarService _calendarService;

    public AppointmentManager(IAppointmentRepository appointmentRepository, ICalendarService calendarService)
    {
        _appointmentRepository = appointmentRepository;
        _calendarService = calendarService;
    }

    public async Task<IEnumerable<Appointment>> GetListByDateRange(DateOnly startDate, DateOnly endDate)
    {
        var startTime = startDate.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
        var endTime = endDate.ToDateTime(TimeOnly.MaxValue).ToUniversalTime();

        var q = _appointmentRepository
            .Query()
            .Where(a =>
                GetVisibleAppointmentStatuses().Contains(a.Status) &&
                a.StartDate >= startTime &&
                a.EndDate <= endTime
            );

        return await q.ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetListDetailedByDateRange(DateOnly startDate, DateOnly endDate)
    {
        var startTime = startDate.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
        var endTime = endDate.ToDateTime(TimeOnly.MaxValue).ToUniversalTime();

        var q = _appointmentRepository
            .Query()
            .Include(a => a.Client)
            .Where(a =>
                a.StartDate >= startTime &&
                a.EndDate <= endTime
            );

        return await q.ToListAsync();
    }

    private CalendarEvent CreateCalendarEvent(Appointment appointment)
    {
        var appointmentStatusProps = GetAppointmentStatus(appointment.Status);
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
        var appointmentStatusProps = GetAppointmentStatus(appointment.Status);
        var color = appointmentStatusProps.ColorId;
        var calendarEvent =
            await _calendarService.UpdateEventColor(appointment.CalendarEventId, color, cancellationToken);
        if (!calendarEvent.Color.Equals(color))
        {
            throw new BusinessException(AppointmentsMessages.FailedColorUpdate);
        }
    }

    public AppointmentStatus[] GetVisibleAppointmentStatuses()
    {
        return GetAppointmentStatuses()
            .Values
            .Where(s => s.IsVisible)
            .Select(s => s.Status)
            .ToArray();
    }

    public AppointmentStatusProps GetAppointmentStatus(AppointmentStatus status)
    {
        var statuses = GetAppointmentStatuses();
        return statuses[status];
    }

    public IDictionary<AppointmentStatus, AppointmentStatusProps> GetAppointmentStatuses()
    {
        return new Dictionary<AppointmentStatus, AppointmentStatusProps>
        {
            {
                AppointmentStatus.Pending,
                new AppointmentStatusProps
                {
                    Status = AppointmentStatus.Pending,
                    ColorId = "8",
                    ColorCode = "gray",
                    Description = AppointmentsMessages.AppointmentStatusPending,
                    IsVisible = true
                }
            },
            {
                AppointmentStatus.Confirmed,
                new AppointmentStatusProps
                {
                    Status = AppointmentStatus.Confirmed,
                    ColorId = "11",
                    ColorCode = "red",
                    Description = AppointmentsMessages.AppointmentStatusConfirmed,
                    IsVisible = true
                }
            },
            {
                AppointmentStatus.Cancelled,
                new AppointmentStatusProps
                {
                    Status = AppointmentStatus.Cancelled,
                    ColorId = "9",
                    ColorCode = "blue",
                    Description = AppointmentsMessages.AppointmentStatusCancelled,
                    IsVisible = false
                }
            },
            {
                AppointmentStatus.Busy,
                new AppointmentStatusProps
                {
                    Status = AppointmentStatus.Busy,
                    ColorId = "5",
                    ColorCode = "orange",
                    Description = AppointmentsMessages.AppointmentStatusBusy,
                    IsVisible = true
                }
            }
        };
    }
}