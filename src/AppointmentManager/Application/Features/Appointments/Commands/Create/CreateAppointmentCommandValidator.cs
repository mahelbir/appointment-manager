using FluentValidation;

namespace Application.Features.Appointments.Commands.Create;

public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentCommandValidator()
    {
        RuleFor(p => p.StartDate)
            .NotEmpty()
            .NotNull()
            .LessThan(p => p.EndDate);

        RuleFor(p => p.EndDate)
            .NotEmpty()
            .NotNull()
            .GreaterThan(p => p.StartDate);

        RuleFor(p => p.Status)
            .NotEmpty()
            .NotNull();
    }
}