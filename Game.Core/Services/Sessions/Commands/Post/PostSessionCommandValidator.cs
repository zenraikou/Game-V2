using FluentValidation;

namespace Game.Core.Services.Sessions.Commands.Post;

public class PostSessionCommandValidator : AbstractValidator<PostSessionCommand>
{
    public PostSessionCommandValidator()
    {
        RuleFor(x => x.Session.Fingerprint).NotEmpty();
        RuleFor(x => x.Session.Expiry).NotEmpty();
    }
}
