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
        var fingerprint = _httpContextAccessor.HttpContext?.Request.Headers["Fingerprint"].ToString();
        var jwt = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var handler = new JwtSecurityTokenHandler();

        if (handler.CanReadToken(jwt) is false) throw new UnauthorizedException("Invalid JWT.");

        var jti = handler.ReadJwtToken(jwt).Claims.FirstOrDefault(c => c.Type == "jti")?.Value.ToString();
        var session = await _sessionRepository.Get(s => s.JTI == jti);

        if (fingerprint is null) throw new UnauthorizedException("Fingerprint not found.");
        if (session is null) throw new UnauthorizedException("Session not found.");
        Console.WriteLine($"JTI: {session.JTI}, Fingerprint: {session.Fingerprint}");
        if (session.Fingerprint.Equals(fingerprint) is false) throw new UnauthorizedException("Fingerprint is invalid.");
        await Task.CompletedTask;
    }
}
