using Game.Contracts.Session;
using Game.Core.Common.Constants;
using Game.Core.Common.Interfaces.Time;
using Game.Core.Common.Settings;
using Game.Core.Services.Authentication.Commands;
using Game.Core.Services.Authentication.Queries;
using MediatR;
using Microsoft.Extensions.Options;

namespace Game.Core.Services.Authentication.Handlers;

public class GenerateSessionHandler : IRequestHandler<GenerateSessionCommand, SessionResponse>
{
    private readonly ISender _mediator;
    private readonly ITime _time;
    private readonly JWTSettings _jwtSettings;

    public GenerateSessionHandler(ISender mediator, ITime time, IOptions<JWTSettings> jwtSettings)
    {
        _mediator = mediator;
        _time = time;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<SessionResponse> Handle(GenerateSessionCommand request, CancellationToken cancellationToken)
    {
        var getFingerprintQuery = new GetFingerprintQuery();
        var fingerprint = await _mediator.Send(getFingerprintQuery);

        var getClaimQuery = new GetClaimQuery(c => c.Type == JWTClaims.JTI, request.JWT);
        var jti = await _mediator.Send(getClaimQuery);

        var response = new SessionResponse
        {
            Id = Guid.Parse(jti),
            Fingerprint = fingerprint,
            Expiry = _time.Now.AddDays(_jwtSettings.Expiry)
        };

        return await Task.FromResult(response);
    }
}
