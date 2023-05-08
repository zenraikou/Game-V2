using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Common.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Game.Infrastructure.Authentication;

public class JWTGenerator : IJWTGenerator
{
    private readonly JWTSettings _jwtSettings;
    private readonly IDateTImeProvider _dateTimeProvider;

    public JWTGenerator(IOptions<JWTSettings> jwtSettings, IDateTImeProvider dateTimeProvider)
    {
        _jwtSettings = jwtSettings.Value;
        _dateTimeProvider = dateTimeProvider;
    }

    public string GenerateToken(Guid id, string name, string uniqueName)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, name),
            new Claim(JwtRegisteredClaimNames.UniqueName, uniqueName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _dateTimeProvider.Now.AddDays(_jwtSettings.ExpiryDays),
            claims: claims,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
