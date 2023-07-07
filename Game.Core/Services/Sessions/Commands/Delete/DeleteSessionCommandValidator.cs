using FluentValidation;

namespace Game.Core.Services.Sessions.Commands.Delete;

public class DeleteSessionCommandValidator : AbstractValidator<DeleteSessionCommand>
{
    public DeleteSessionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
