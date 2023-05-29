using System.IdentityModel.Tokens.Jwt;
using Game.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Game.Core.Services.Claims;

public class GetClaimHandler : IRequestHandler<GetClaimCommand, string>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetClaimHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> Handle(GetClaimCommand request, CancellationToken cancellationToken)
    {
        var jwt = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var claim = new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims.FirstOrDefault(request.Expression)?.Value;

        if (claim is null)
        {
            throw new UnauthorizedException("Access denied.");
        }

        return await Task.FromResult(claim);
    }
}
