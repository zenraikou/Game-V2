using System.IdentityModel.Tokens.Jwt;
using Game.Contracts.Generator.GenerateSession;
using Game.Contracts.Session;
using Game.Core.Common.Settings;
using Game.Core.Exceptions;
using Game.Core.TempServices.Time;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Game.Core.Services.Generator.GenerateSession;

public class GenerateSessionHandler : IRequestHandler<GenerateSessionCommand, GenerateSessionResponse>
{
    private readonly ITime _time;
    private readonly JWTSettings _jwtSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public GenerateSessionHandler(
        ITime time, 
        IOptions<JWTSettings> jwtSettings, 
        IHttpContextAccessor httpContextAccessor, 
        IMapper mapper)
    {
        _time = time;
        _jwtSettings = jwtSettings.Value;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<GenerateSessionResponse> Handle(GenerateSessionCommand request, CancellationToken cancellationToken)
    {
        var jti = new JwtSecurityTokenHandler().ReadJwtToken(request.GenerateSession.JWT).Claims.FirstOrDefault(c => c.Type == "jti")!.Value.ToString();
        var fingerprint = _httpContextAccessor.HttpContext?.Request.Headers["Fingerprint"].ToString();

        if (fingerprint is null)
        {
            throw new UnauthorizedException("Access denied.");
        }

        var sessionResponse = new SessionResponse
        {
            JTI = jti,
            Fingerprint = fingerprint,
            Expiry = _time.Now.AddDays(_jwtSettings.Expiry)
        };

        var response = _mapper.Map<GenerateSessionResponse>(sessionResponse);
        return await Task.FromResult(response);
    }
}
