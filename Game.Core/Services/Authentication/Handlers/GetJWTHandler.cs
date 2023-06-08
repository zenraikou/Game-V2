using System.IdentityModel.Tokens.Jwt;
using Game.Core.Common.Constants;
using Game.Core.Exceptions;
using Game.Core.Services.Authentication.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Game.Core.Services.Authentication.Handlers;

public class GetJWTHandler : IRequestHandler<GetJWTQuery, string>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetJWTHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> Handle(GetJWTQuery request, CancellationToken cancellationToken)
    {
        var jwt = _httpContextAccessor.HttpContext?.Request.Headers[HTTPHeaders.Authorization].ToString().Split(' ')[1];
        var handler = new JwtSecurityTokenHandler();

        if (string.IsNullOrEmpty(jwt) || !handler.CanReadToken(jwt))
        {
            throw new UnauthorizedException("Access denied.");
        }

        return await Task.FromResult(jwt);
    }
}
