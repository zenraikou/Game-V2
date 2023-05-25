using Game.Domain.Entities;
using MediatR;

namespace Game.Core.Services.Fingerprinting;

public class CheckFingerprintHandler : IRequestHandler<CheckFingerprintCommand, Session>
{
    public Task<Session> Handle(CheckFingerprintCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
