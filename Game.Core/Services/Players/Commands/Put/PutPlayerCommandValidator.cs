using FluentValidation;

namespace Game.Core.Services.Players.Commands.Put;

public class PutPlayerCommandValidator : AbstractValidator<PutPlayerCommand>
{
    public PutPlayerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Player.Handle).NotEmpty();
        RuleFor(x => x.Player.Name).NotEmpty();
        RuleFor(x => x.Player.UniqueName).NotEmpty();
        RuleFor(x => x.Player.Email).NotEmpty();
        RuleFor(x => x.Player.Password).NotEmpty();
        RuleFor(x => x.Player.Role).NotEmpty();
    }
}
