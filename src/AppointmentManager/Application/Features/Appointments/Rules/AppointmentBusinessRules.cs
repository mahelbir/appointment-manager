using Application.Extensions;
using Application.Features.Appointments.Constants;
using Application.Services.AppointmentService;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;
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

    public async Task<Appointment> ShouldBeExistId(int id)
    {
        var appointment = await _appointmentRepository.GetAsync(
            predicate: a => a.Id == id
        );
        if (appointment == null)
            throw new BusinessException(AppointmentsMessages.DontExists);
        return appointment;
    }

    public void DateRangeCantTooLarge(DateOnly startDate, DateOnly endDate)
    {
        var timeSpan = endDate.UtcMax() -
                       startDate.UtcMin();
        if (timeSpan.TotalDays > 31)
        {
            throw new BusinessException(AppointmentsMessages.DateRangeTooLarge);
        }
    }

    public void CantPastTime(DateTime startDate, DateTime endDate)
    {
        startDate = startDate.ToUniversalTime();
        endDate = endDate.ToUniversalTime();

        if (startDate < DateTime.UtcNow || endDate < DateTime.UtcNow)
        {
            throw new BusinessException(AppointmentsMessages.PastTime);
        }
    }

    public void CantLessTime(DateTime startDate, DateTime endDate, DateTime appointmentStartDate,
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
        bool isOverlap = (id > 0)
            ? await _appointmentRepository.IsOverlap(startDate, endDate, id)
            : await _appointmentRepository.IsOverlap(startDate, endDate);
        if (isOverlap)
        {
            throw new BusinessException(AppointmentsMessages.Overlap);
        }
    }

    public void CantCancelled(Appointment appointment)
    {
        if (appointment.Status == AppointmentStatus.Cancelled)
        {
            throw new BusinessException(AppointmentsMessages.InvalidStatus);
        }
    }

    public void CantEmpty(string? t)
    {
        if (string.IsNullOrEmpty(t))
        {
            throw new BusinessException(AppointmentsMessages.Wrong);
        }
    }
    
}