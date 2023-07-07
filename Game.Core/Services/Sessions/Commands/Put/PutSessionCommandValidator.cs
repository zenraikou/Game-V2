using FluentValidation;
using Game.Core.Services.Sessions.Commands.Put;

namespace Game.Core;

public class PutSessionCommandValidator : AbstractValidator<PutSessionCommand>
{
    public PutSessionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Session.Fingerprint).NotEmpty();
        RuleFor(x => x.Session.Expiry).NotEmpty();
    }
}
