using Application.Features.Appointments.Constants;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;

namespace Application.Features.Appointments.Rules;

public class AppointmentBusinessRules : BaseBusinessRules
{
    public async Task ShouldBeExistsWhenSelected(Appointment? appointment)
    {
        if (appointment == null)
            throw new BusinessException(AppointmentsMessages.DontExists);
    }
}