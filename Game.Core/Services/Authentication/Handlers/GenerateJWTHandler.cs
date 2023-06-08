using Game.Core.Common.Interfaces.Time;
using Game.Core.Common.Constants;
using Game.Core.Common.Settings;
using Game.Core.Services.Authentication.Commands;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Game.Core.Services.Authentication.Handlers;

public class GenerateJWTHandler : IRequestHandler<GenerateJWTCommand, string>
{
    private readonly ITime _time;
    private readonly JWTSettings _jwtSettings;

    public GenerateJWTHandler(ITime time, IOptions<JWTSettings> jwtSettings)
    {
        _time = time;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<string> Handle(GenerateJWTCommand request, CancellationToken cancellationToken)
    {
        var claims = new Claim[]
        {
            new Claim(JWTClaims.JTI, request.JTI ?? Guid.NewGuid().ToString()),
            new Claim(JWTClaims.Id, request.Id),
            new Claim(JWTClaims.Role, request.Role)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _time.Now.AddMinutes(_jwtSettings.Expiry));

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.WriteToken(securityToken);

        var response = jwt;
        return await Task.FromResult(response);
    }
}
