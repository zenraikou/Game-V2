using System.IdentityModel.Tokens.Jwt;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Game.Core.TempServices.Fingerprinting;

public class FingerprintingService : IFingerprintingService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;

    public FingerprintingService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
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
        var session = await _unitOfWork.Sessions.Get(s => s.JTI == jti);

        if (session is null || !session.Fingerprint.Equals(fingerprint))
        {
            throw new UnauthorizedException("Invalid credentials.");
        }
    }
}
