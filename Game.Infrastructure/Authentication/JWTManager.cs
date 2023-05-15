using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Game.Core.Common;
using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Common.Interfaces.Services;
using Game.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Game.Infrastructure.Authentication;

public class JWTManager : IJWTManager
{
    private readonly IDateTImeProvider _dateTimeProvider;
    private readonly JWTSettings _jwtSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JWTManager(
        IDateTImeProvider dateTimeProvider,
        IOptions<JWTSettings> jwtSettings, 
        IHttpContextAccessor httpContextAccessor)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtSettings.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim("handle", user.Handle),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UniqueName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("role", user.Role.ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _dateTimeProvider.Now.AddHours(_jwtSettings.Expiry));

        var jwt = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return jwt;
    }

    public RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expiry = _dateTimeProvider.Now.AddDays(7)
        };

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = refreshToken.Expiry
        };

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

        return refreshToken;
    }

    public void SetRefreshToken(Guid id, RefreshToken refreshToken)
    {
        var user = InMemory.Users.FirstOrDefault(u => u.Id == id);

        if (user is null)  throw new Exception("User is null.");

        user.RefreshToken = refreshToken.Token;
        user.TokenExpiry = refreshToken.Expiry;
        user.TokenCreationStamp = refreshToken.CreationStamp;
    }
}
