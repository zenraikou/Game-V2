using Game.Core.Common.Constants;
using Game.Core.Exceptions;
using Game.Core.Services.Authentication.Commands;
using Game.Core.Services.Authentication.Queries;
using Game.Core.Services.Sessions.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Game.Core.Services.Authentication.Handlers;

public class FingerprintingHandler : IRequestHandler<FingerprintingCommand, Unit>
{
    private readonly ISender _mediator;

    public FingerprintingHandler(ISender mediator)
    {
        _mediator = mediator;
    }

    public async Task<Unit> Handle(FingerprintingCommand request, CancellationToken cancellationToken)
    {
        var getJWTQuery = new GetJWTQuery();
        var jwt = await _mediator.Send(getJWTQuery);

        var getFingerprintQuery = new GetFingerprintQuery();
        var fingerprint = await _mediator.Send(getFingerprintQuery);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        var jti = token.Claims.FirstOrDefault(c => c.Type == JWTClaims.JTI)!.Value;

        var getSessionQuery = new GetSessionQuery(s => s.Id == Guid.Parse(jti));
        var session = await _mediator.Send(getSessionQuery);

        if (session == null || session.Fingerprint != fingerprint)
        {
            throw new UnauthorizedException("Access denied.");
        }

        return await Unit.Task;
    }
}
