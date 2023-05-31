using Game.Contracts.Session;
using MediatR;

namespace Game.Core.Services.Fingerprinting;

public class FingerprintingHandler : IRequestHandler<FingerprintingCommand, SessionResponse>
{
    public Task<SessionResponse> Handle(FingerprintingCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
