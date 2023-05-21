using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Common.Interfaces.Time;
using Game.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Game.Infrastructure.Authentication;

public class TokenService : ITokenService
{
    private readonly JWTSettings _jwtSettings;
    private readonly ITime _time;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(IOptions<JWTSettings> jwtSettings, ITime time, IHttpContextAccessor httpContextAccessor)
    {
        _jwtSettings = jwtSettings.Value;
        _time = time;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateJWT(User user)
    {
        var claims = new List<Claim>
        {
            new Claim("jti", Guid.NewGuid().ToString()),
            new Claim("handle", user.Handle),
            new Claim("role", user.Role.ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _time.Now.AddMinutes(_jwtSettings.Expiry));

        var jwt = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return jwt;
    }

    public Session GenerateSession(string jwt)
    {
        var jti = new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims.FirstOrDefault(c => c.Type == "jti")!.Value.ToString();
        var fingerprint = _httpContextAccessor.HttpContext?.Request.Headers["Fingerprint"].ToString();

        if (fingerprint is null) throw new Exception("Fingerprint is null.");

        var session = new Session
        {
            JTI = jti,
            Fingerprint = fingerprint,
            Expiry = _time.Now.AddDays(_jwtSettings.Expiry)
        };

        return session;
    }
}
