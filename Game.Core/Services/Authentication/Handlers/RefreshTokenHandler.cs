using ErrorOr;
using Game.Contracts.Authentication;
using Game.Core.Common.Constants;
using Game.Core.Common.Interfaces.Time;
using Game.Core.Services.Authentication.Commands;
using Game.Core.Services.Authentication.Queries;
using Game.Core.Services.Sessions.Queries;
using Game.Domain.Common.Errors;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Game.Core.Services.RefreshToken;

public record RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<AuthenticationResponse>>
{
    private readonly ILogger<RefreshTokenHandler> _logger;
    private readonly ISender _mediator;
    private readonly ITime _time;

    public RefreshTokenHandler(ILogger<RefreshTokenHandler> logger, ISender mediator, ITime time)
    {
        _logger = logger;
        _mediator = mediator;
        _time = time;
    }

    public async Task<ErrorOr<AuthenticationResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var fingerprint = await _mediator.Send(new GetFingerprintQuery());
        var jti = await _mediator.Send(new GetClaimQuery(c => c.Type == JWTClaims.JTI));
        var id = await _mediator.Send(new GetClaimQuery(c => c.Type == JWTClaims.Id));
        var role = await _mediator.Send(new GetClaimQuery(c => c.Type == JWTClaims.Role));

        var expiry = await _mediator.Send(new GetClaimQuery(c => c.Type == JWTClaims.Expiry));
        var expiryUnix = long.Parse(expiry.Value);
        var expiryUtc = DateTimeOffset.FromUnixTimeSeconds(expiryUnix).UtcDateTime;

        var sessionResponse = await _mediator.Send(new GetSessionQuery(s => s.Id == Guid.Parse(jti.Value)));

        if (sessionResponse.Value.Fingerprint != fingerprint.Value || _time.Now <= expiryUtc || _time.Now >= sessionResponse.Value.Expiry)
        {
            return Errors.Authorization.Unauthorized;
        }

        var jwt = await _mediator.Send(new GenerateJWTCommand(id.Value, role.Value, jti.Value));

        var response = new AuthenticationResponse { JWT = jwt };
        return response;
    }
}
