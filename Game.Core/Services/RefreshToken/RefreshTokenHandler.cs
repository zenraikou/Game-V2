using Game.Contracts.Authentication;
using Game.Contracts.Generator.GenerateJWT;
using Game.Core.Common.Headers;
using Game.Core.Common.Interfaces.Time;
using Game.Core.Common.JWT;
using Game.Core.Exceptions;
using Game.Core.Services.Claims;
using Game.Core.Services.Generator.GenerateJWT;
using Game.Core.Services.Header;
using Game.Core.Services.Sessions.Get;
using MediatR;

namespace Game.Core.Services.RefreshToken;

public record RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthenticationResponse>
{
    private readonly ISender _mediator;
    private readonly ITime _time;

    public RefreshTokenHandler(ISender mediator, ITime time)
    {
        _mediator = mediator;
        _time = time;
    }

    public async Task<AuthenticationResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var getHeaderQuery = new GetHeaderQuery(Headers.Fingerprint);
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
        
        var getSessionQuery = new GetSessionQuery(s => s.JTI == jti);
        var sessionResponse = await _mediator.Send(getSessionQuery);

        if (sessionResponse is null || sessionResponse.Fingerprint != fingerprint/* || sessionResponse.Expiry < expiry*/)
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
