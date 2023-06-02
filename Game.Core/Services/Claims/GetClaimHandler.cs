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
        var context = _httpContextAccessor.HttpContext;
        var jwt = context?.Request.Headers["Authorization"].ToString().Split(' ')[1];

        var handler = new JwtSecurityTokenHandler();
        var claim = handler.ReadJwtToken(jwt).Claims.FirstOrDefault(request.Expression)?.Value;

        if (string.IsNullOrEmpty(claim))
        {
            throw new UnauthorizedException("Access denied.");
        }

        return await Task.FromResult(claim);
    }
}
