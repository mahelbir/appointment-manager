using FluentValidation;

namespace Application.Features.Appointments.Commands.Busy;

public class BusyAppointmentCommandValidator : AbstractValidator<BusyAppointmentCommand>
{
    public BusyAppointmentCommandValidator()
    {
        RuleFor(a => a.StartDate)
            .NotEmpty()
            .NotNull()
            .LessThan(a => a.EndDate);

        RuleFor(a => a.EndDate)
            .NotEmpty()
            .NotNull()
            .GreaterThan(a => a.StartDate);
    }
}