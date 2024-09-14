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

        RuleFor(a => a.Client.FirstName)
            .NotEmpty()
            .NotNull();

        RuleFor(a => a.Client.LastName)
            .NotEmpty()
            .NotNull();

        RuleFor(a => a.Client.Contact)
            .NotEmpty()
            .NotNull()
            .MinimumLength(5);
    }
}