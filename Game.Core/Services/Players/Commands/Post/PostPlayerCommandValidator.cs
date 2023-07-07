using FluentValidation;

namespace Game.Core.Services.Players.Commands.Post;

public class PostPlayerCommandValidator : AbstractValidator<PostPlayerCommand>
{
    public PostPlayerCommandValidator()
    {
        RuleFor(x => x.Player.Handle).NotEmpty();
        RuleFor(x => x.Player.Name).NotEmpty();
        RuleFor(x => x.Player.UniqueName).NotEmpty();
        RuleFor(x => x.Player.Email).NotEmpty();
        RuleFor(x => x.Player.Password).NotEmpty();
        RuleFor(x => x.Player.Role).NotEmpty();
    }
}
