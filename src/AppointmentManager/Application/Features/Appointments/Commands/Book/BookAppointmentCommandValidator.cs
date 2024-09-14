using FluentValidation;

namespace Application.Features.Appointments.Commands.Book;

public class BookAppointmentCommandValidator : AbstractValidator<BookAppointmentCommand>
{
    public BookAppointmentCommandValidator()
    {
        RuleFor(p => p.StartDate)
            .NotEmpty()
            .NotNull()
            .LessThan(p => p.EndDate);

        RuleFor(p => p.EndDate)
            .NotEmpty()
            .NotNull()
            .GreaterThan(p => p.StartDate);

        RuleFor(p => p.Client.FirstName)
            .NotEmpty()
            .NotNull();

        RuleFor(p => p.Client.LastName)
            .NotEmpty()
            .NotNull();

        RuleFor(p => p.Client.Contact)
            .NotEmpty()
            .NotNull()
            .MinimumLength(5);
    }
}