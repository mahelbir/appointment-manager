using Application.Features.Appointments.Constants;
using Application.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;

namespace Application.Services.AppointmentService;

public class AppointmentManager : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentManager(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<IEnumerable<Appointment>> GetListByDateRange(DateOnly startDate, DateOnly endDate)
    {
        var startTime = startDate.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
        var endTime = endDate.ToDateTime(TimeOnly.MaxValue).ToUniversalTime();

        return await _appointmentRepository
            .Query()
            .Where(a =>
                GetVisibleAppointmentStatuses().Contains(a.Status) &&
                a.StartDate >= startTime &&
                a.EndDate <= endTime
            )
            .ToListAsync();
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
                AppointmentStatus.Busy,
                new AppointmentStatusProps
                {
                    Status = AppointmentStatus.Busy,
                    ColorId = "5",
                    ColorCode = "orange",
                    Description = AppointmentsMessages.AppointmentStatusBusy,
                    IsVisible = true
                }
            },
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
            }
        };
    }
    
}