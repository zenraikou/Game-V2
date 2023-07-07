using FluentValidation;
using Game.Core.Services.Sessions.Commands.Patch;

namespace Game.Core;

public class PatchSessionCommandValidator : AbstractValidator<PatchSessionCommand>
{
    public PatchSessionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Session.Fingerprint).NotEmpty();
        RuleFor(x => x.Session.Expiry).NotEmpty();
    }
}
