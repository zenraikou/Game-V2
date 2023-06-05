using Game.Contracts.Generator.GenerateSession;
using Game.Contracts.Session;
using Game.Core.Common.Headers;
using Game.Core.Common.Interfaces.Time;
using Game.Core.Common.JWT;
using Game.Core.Exceptions;
using Game.Core.Services.Claims;
using Game.Core.Services.Header;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Game.Core.Services.Generator.GenerateSession;

public class GenerateSessionHandler : IRequestHandler<GenerateSessionCommand, GenerateSessionResponse>
{
    private readonly ISender _mediator;
    private readonly ITime _time;
    private readonly JWTSettings _jwtSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public GenerateSessionHandler(
        ISender mediator, 
        ITime time, 
        IOptions<JWTSettings> jwtSettings, 
        IHttpContextAccessor httpContextAccessor, 
        IMapper mapper)
    {
        _mediator = mediator;
        _time = time;
        _jwtSettings = jwtSettings.Value;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<GenerateSessionResponse> Handle(GenerateSessionCommand request, CancellationToken cancellationToken)
    {
        var getHeaderQuery = new GetHeaderQuery(Headers.Fingerprint);
        var fingerprint = await _mediator.Send(getHeaderQuery);

        var getClaimQuery = new GetClaimQuery(c => c.Type == JWTClaims.JTI, request.GenerateSession.JWT);
        var jti = await _mediator.Send(getClaimQuery);

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
