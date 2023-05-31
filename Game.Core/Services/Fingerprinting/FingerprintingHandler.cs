using System.IdentityModel.Tokens.Jwt;
using Game.Core.Common.Interfaces.Persistence;
using Game.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Game.Core.Services.Fingerprinting;

public class FingerprintingHandler : IRequestHandler<FingerprintingCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FingerprintingHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Unit> Handle(FingerprintingCommand request, CancellationToken cancellationToken)
    {
        var jwt = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var fingerprint = _httpContextAccessor.HttpContext?.Request.Headers["Fingerprint"].ToString();

        if (string.IsNullOrEmpty(jwt) || string.IsNullOrEmpty(fingerprint))
        {
            throw new UnauthorizedException("Access denied.");
        }

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        var jti = token.Claims.FirstOrDefault(c => c.Type == "jti")?.Value;
        var session = await _unitOfWork.Sessions.Get(s => s.JTI == jti);

        if (session is null || !session.Fingerprint.Equals(fingerprint))
        {
            throw new UnauthorizedException("Access denied.");
        }

        return Unit.Value;
    }
}
