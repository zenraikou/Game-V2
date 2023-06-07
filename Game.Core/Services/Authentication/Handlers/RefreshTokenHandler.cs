using Game.Contracts.Authentication;
using Game.Contracts.Generator.GenerateJWT;
using Game.Core.Common.Constants;
using Game.Core.Common.Interfaces.Time;
using Game.Core.Exceptions;
using Game.Core.Services.Authentication.Commands;
using Game.Core.Services.Authentication.Queries;
using Game.Core.Services.Sessions.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Game.Core.Services.RefreshToken;

public record RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthenticationResponse>
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

    public async Task<AuthenticationResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var getHeaderQuery = new GetHeaderQuery(HTTPHeaders.Fingerprint);
        var fingerprint = await _mediator.Send(getHeaderQuery);

        if (string.IsNullOrEmpty(fingerprint))
        {
            throw new UnauthorizedException("Access denied.");
        }

        var getClaimQuery = new GetClaimQuery(c => c.Type == JWTClaims.JTI);
        var jti = await _mediator.Send(getClaimQuery);

        getClaimQuery = new GetClaimQuery(c => c.Type == JWTClaims.Id);
        var id = await _mediator.Send(getClaimQuery);

        getClaimQuery = new GetClaimQuery(c => c.Type == JWTClaims.Role);
        var role = await _mediator.Send(getClaimQuery);

        getClaimQuery = new GetClaimQuery(c => c.Type == JWTClaims.Expiry);
        var expiry = await _mediator.Send(getClaimQuery);

        var expiryUnix = long.Parse(expiry);
        var expiryUtc = DateTimeOffset.FromUnixTimeSeconds(expiryUnix).UtcDateTime;
        
        var getSessionQuery = new GetSessionQuery(s => s.Id == Guid.Parse(jti));
        var sessionResponse = await _mediator.Send(getSessionQuery);

        if (sessionResponse is null || sessionResponse.Fingerprint != fingerprint || _time.Now <= expiryUtc || _time.Now >= sessionResponse.Expiry)
        {
            throw new UnauthorizedException("Access denied.");
        }

        var generateJWTRequest = new GenerateJWTRequest { Id = id, Role = role, JTI = jti };
        var generateJWTCommand = new GenerateJWTCommand(generateJWTRequest);
        var generateJWTResponse = await _mediator.Send(generateJWTCommand);

        var response = new AuthenticationResponse { JWT = generateJWTResponse.JWT };
        return response;
    }
}
