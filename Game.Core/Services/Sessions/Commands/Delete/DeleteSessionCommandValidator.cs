using FluentValidation;
using Game.Core.Services.Sessions.Commands.Delete;

namespace Game.Core;

public class DeleteSessionCommandValidator : AbstractValidator<DeleteSessionCommand>
{
    public DeleteSessionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
