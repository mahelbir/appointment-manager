using Application.Features.Appointments.Constants;
using Application.Services.AppointmentService;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;

namespace Application.Features.Appointments.Rules;

public class AppointmentBusinessRules : BaseBusinessRules
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IAppointmentService _appointmentService;


    public AppointmentBusinessRules(IAppointmentRepository appointmentRepository,
        IAppointmentService appointmentService)
    {
        _appointmentRepository = appointmentRepository;
        _appointmentService = appointmentService;
    }

    public async Task ShouldBeExists(Appointment? appointment)
    {
        if (appointment == null)
            throw new BusinessException(AppointmentsMessages.DontExists);
    }

    public async Task DateRangeCantTooLarge(DateOnly startDate, DateOnly endDate)
    {
        var timeSpan = endDate.ToDateTime(TimeOnly.MaxValue).ToUniversalTime() -
                       startDate.ToDateTime(TimeOnly.MinValue).ToUniversalTime();
        if (timeSpan.TotalDays > 31)
        {
            throw new BusinessException(AppointmentsMessages.DateRangeTooLarge);
        }
    }

    public async Task CantPastTime(DateTime startDate, DateTime endDate)
    {
        startDate = startDate.ToUniversalTime();
        endDate = endDate.ToUniversalTime();

        if (startDate < DateTime.UtcNow || endDate < DateTime.UtcNow)
        {
            throw new BusinessException(AppointmentsMessages.PastTime);
        }
    }

    public async Task CantLessTime(DateTime startDate, DateTime endDate, DateTime appointmentStartDate,
        DateTime appointmentEndDate)
    {
        startDate = startDate.ToUniversalTime();
        endDate = endDate.ToUniversalTime();

        if (startDate < appointmentStartDate || endDate < appointmentEndDate)
        {
            throw new BusinessException(AppointmentsMessages.PastTime);
        }
    }

    public async Task CantOverlap(DateTime startDate, DateTime endDate, int id = 0)
    {
        startDate = startDate.ToUniversalTime();
        endDate = endDate.ToUniversalTime();

        if (id)
        {
            if (_appointmentRepository.IsOverlap())
            {
                
            }
        }

        if (appointment == null) return;

        if (id == 0 || id != appointment.Id)
        {
            throw new BusinessException(AppointmentsMessages.Overlap);
        }
    }
    
    public async Task CantCancelled(Appointment appointment)
    {
        if (appointment.Status == AppointmentStatus.Cancelled)
        {
            throw new BusinessException(AppointmentsMessages.InvalidStatus);
        }
    }
    
    public async Task CantEmpty(string? t)
    {
        if (string.IsNullOrEmpty(t))
        {
            throw new BusinessException(AppointmentsMessages.Wrong);
        }
    }
    
    public async Task TokenShouldMatch(string calendarToken, string? requestToken)
    {
        if (calendarToken != requestToken)
        {
            throw new BusinessException(AppointmentsMessages.NotMatch);
        }
    }
    
}