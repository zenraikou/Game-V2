using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Game.Contracts.Generator.GenerateJWT;
using Game.Core.Common.Settings;
using Game.Core.TempServices.Time;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Game.Core.Services.Generator.GenerateJWT;

public class GenerateJWTHandler : IRequestHandler<GenerateJWTCommand, GenerateJWTResponse>
{
    private readonly ITime _time;
    private readonly JWTSettings _jwtSettings;
    private readonly IMapper _mapper;

    public GenerateJWTHandler(ITime time, IOptions<JWTSettings> jwtSettings, IMapper mapper)
    {
        _time = time;
        _jwtSettings = jwtSettings.Value;
        _mapper = mapper;
    }

    public async Task<GenerateJWTResponse> Handle(GenerateJWTCommand request, CancellationToken cancellationToken)
    {
        var claims = new Claim[]
        {
            new Claim("id", request.GenerateJWT.Id.ToString()),
            new Claim("role", request.GenerateJWT.Role.ToString()),
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

        var response = new GenerateJWTResponse { JWT = jwt };
        return await Task.FromResult(response);
    }
}
