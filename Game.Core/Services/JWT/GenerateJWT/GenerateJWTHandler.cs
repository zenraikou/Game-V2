using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Game.Core.Common.Settings;
using Game.Core.TempServices.Time;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Game.Core.Services.JWT.GenerateJWT;

public class GenerateJWTHandler : IRequestHandler<GenerateJWTCommand, string>
{
    private readonly ITime _time;
    private readonly JWTSettings _jwtSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GenerateJWTHandler(ITime time, IOptions<JWTSettings> jwtSettings, IHttpContextAccessor httpContextAccessor)
    {
        _time = time;
        _jwtSettings = jwtSettings.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> Handle(GenerateJWTCommand request, CancellationToken cancellationToken)
    {
        var claims = new Claim[]
        {
            new Claim("id", request.Player.Id.ToString()),
            new Claim("role", request.Player.Role),
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
        return await Task.FromResult(jwt);
    }
}
