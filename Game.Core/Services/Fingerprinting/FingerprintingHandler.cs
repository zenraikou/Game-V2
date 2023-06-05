using System.IdentityModel.Tokens.Jwt;
using Game.Core.Common.Headers;
using Game.Core.Exceptions;
using Game.Core.Services.Sessions.Get;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Game.Core.Services.Fingerprinting;

public class FingerprintingHandler : IRequestHandler<FingerprintingCommand, Unit>
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FingerprintingHandler(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Unit> Handle(FingerprintingCommand request, CancellationToken cancellationToken)
    {
        var jwt = _httpContextAccessor.HttpContext?.Request.Headers[Headers.Authorization].ToString().Split(' ')[1];
        var fingerprint = _httpContextAccessor.HttpContext?.Request.Headers[Headers.Fingerprint].ToString();

        if (string.IsNullOrEmpty(jwt) || string.IsNullOrEmpty(fingerprint))
        {
            throw new UnauthorizedException("Access denied.");
        }

        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(jwt))
        {
            throw new UnauthorizedException("Access denied.");
        }

        var token = handler.ReadJwtToken(jwt);
        var jti = token.Claims.FirstOrDefault(c => c.Type == "jti")?.Value;

        var getSessionQuery = new GetSessionQuery(s => s.JTI == jti);
        var session = await _mediator.Send(getSessionQuery);

        if (session is null || !session.Fingerprint.Equals(fingerprint))
        {
            throw new UnauthorizedException("Access denied.");
        }

        return await Unit.Task;
    }
}
