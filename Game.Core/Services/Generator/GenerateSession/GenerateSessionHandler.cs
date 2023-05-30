using System.IdentityModel.Tokens.Jwt;
using Game.Core.Common.Settings;
using Game.Core.Exceptions;
using Game.Core.TempServices.Time;
using Game.Domain.Entities;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Game.Core.Services.Generator.GenerateSession;

public class GenerateSessionHandler : IRequestHandler<GenerateSessionCommand, Session>
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

    public async Task<Session> Handle(GenerateSessionCommand request, CancellationToken cancellationToken)
    {
        var jti = new JwtSecurityTokenHandler().ReadJwtToken(request.JWT).Claims.FirstOrDefault(c => c.Type == "jti")!.Value.ToString();
        var fingerprint = _httpContextAccessor.HttpContext?.Request.Headers["Fingerprint"].ToString();

        if (fingerprint is null)
        {
            throw new UnauthorizedException("Access denied.");
        }

        var session = new Session
        {
            JTI = jti,
            Fingerprint = fingerprint,
            Expiry = _time.Now.AddDays(_jwtSettings.Expiry)
        };

        // var response = _mapper.Map<GenerateSessionResponse>(sessionResponse);
        return await Task.FromResult(session);
    }
}
