using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Game.Core.Common.Headers;
using Game.Core.Common.JWT;
using Game.Core.TempServices.Time;
using Game.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Game.Core.TempServices.JWT;

public class JWTService : IJWTService
{
    private readonly JWTSettings _jwtSettings;
    private readonly ITime _time;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JWTService(IOptions<JWTSettings> jwtSettings, ITime time, IHttpContextAccessor httpContextAccessor)
    {
        _jwtSettings = jwtSettings.Value;
        _time = time;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateJWT(Player player)
    {
        var claims = new List<Claim>
        {
            new Claim("id", player.Id.ToString()),
            new Claim("role", player.Role.ToString()),
            new Claim("jti", Guid.NewGuid().ToString())
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
        var jti = new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims.FirstOrDefault(c => c.Type == JWTClaims.JTI)!.Value;
        var fingerprint = _httpContextAccessor.HttpContext?.Request.Headers[Headers.Fingerprint].ToString();

        if (fingerprint is null)
        {
            throw new Exception("Fingerprint is null.");
        }

        var session = new Session
        {
            Id = Guid.Parse(jti),
            Fingerprint = fingerprint,
            Expiry = _time.Now.AddDays(_jwtSettings.Expiry)
        };

        return session;
    }
}
