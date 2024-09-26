using FluentValidation;

namespace Application.Features.Appointments.Commands.Update;

public class UpdateAppointmentCommandValidator : AbstractValidator<UpdateAppointmentCommand>
{
    public UpdateAppointmentCommandValidator()
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