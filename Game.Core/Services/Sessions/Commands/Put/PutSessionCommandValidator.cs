using FluentValidation;

namespace Game.Core.Services.Sessions.Commands.Put;

public class PutSessionCommandValidator : AbstractValidator<PutSessionCommand>
{
    public PutSessionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Session.Fingerprint).NotEmpty();
        RuleFor(x => x.Session.Expiry).NotEmpty();
    }
}
