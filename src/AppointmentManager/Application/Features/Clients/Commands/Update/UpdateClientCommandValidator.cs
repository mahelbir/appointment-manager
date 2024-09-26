using FluentValidation;

namespace Application.Features.Clients.Commands.Update;

public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty()
            .NotNull();

        RuleFor(c => c.LastName)
            .NotEmpty()
            .NotNull();

        RuleFor(c => c.Contact)
            .NotEmpty()
            .NotNull()
            .MinimumLength(5);
    }
}