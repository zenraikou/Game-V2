using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Game.Core.Common.Interfaces.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Game.Infrastructure.Authentication;

public class JWTGenerator : IJWTGenerator
{
    public string GenerateToken(Guid id, string name, string uniqueName)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-secret-key")),
            SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, name),
            new Claim(JwtRegisteredClaimNames.UniqueName, uniqueName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var securityToken = new JwtSecurityToken(
            issuer: "Game",
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.Now.AddDays(1));

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
