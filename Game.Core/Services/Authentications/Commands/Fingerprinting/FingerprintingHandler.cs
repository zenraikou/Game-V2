using ErrorOr;
using Game.Core.Common.Constants;
using Game.Core.Services.Authentications.Queries.GetFingerprint;
using Game.Core.Services.Authentications.Queries.GetJWT;
using Game.Core.Services.Sessions.Queries.Get;
using Game.Domain.Common.Errors;
using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace Game.Core.Services.Authentications.Commands.Fingerprinting;

public class FingerprintingHandler : IRequestHandler<FingerprintingCommand, ErrorOr<Success>>
{
    private readonly ISender _mediator;

    public FingerprintingHandler(ISender mediator)
    {
        _mediator = mediator;
    }

    public async Task<ErrorOr<Success>> Handle(FingerprintingCommand request, CancellationToken cancellationToken)
    {
        var jwt = await _mediator.Send(new GetJWTQuery());
        var fingerprint = await _mediator.Send(new GetFingerprintQuery());

        if (jwt.IsError || fingerprint.IsError)
        {
            return Errors.Authorization.Unauthorized;
        }

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt.Value);
        var jti = token.Claims.FirstOrDefault(c => c.Type == JWTClaims.JTI)!.Value;

        var sessionResponse = await _mediator.Send(new GetSessionQuery(s => s.Id == Guid.Parse(jti)));

        if (sessionResponse.Value.Fingerprint != fingerprint.Value)
        {
            return Errors.Authorization.Unauthorized;
        }

        return Result.Success;
    }
}
