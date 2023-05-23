using System.IdentityModel.Tokens.Jwt;
using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Game.Infrastructure.Authentication;

public class FingerprintingService : IFingerprintingService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISessionRepository _sessionRepository;

    public FingerprintingService(IHttpContextAccessor httpContextAccessor, ISessionRepository sessionRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _sessionRepository = sessionRepository;
    }

    public async Task Validate()
    {
        var jwt = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var fingerprint = _httpContextAccessor.HttpContext?.Request.Headers["Fingerprint"].ToString();

        if (jwt is null || fingerprint is null) 
        {
            throw new UnauthorizedException("Invalid credentials.");
        }

        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(jwt))
        {
            throw new UnauthorizedException("Invalid credentials.");
        }

        var token = handler.ReadJwtToken(jwt);
        var jti = token.Claims.FirstOrDefault(c => c.Type == "jti")?.Value;
        var session = await _sessionRepository.Get(s => s.JTI == jti);

        if (session is null || !session.Fingerprint.Equals(fingerprint))
        {
            throw new UnauthorizedException("Invalid credentials.");
        }

        await Task.CompletedTask;
    }
}
