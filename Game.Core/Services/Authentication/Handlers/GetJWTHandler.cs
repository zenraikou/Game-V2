using ErrorOr;
using Game.Core.Common.Constants;
using Game.Core.Services.Authentication.Queries;
using Game.Domain.Common.Errors;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Game.Core.Services.Authentication.Handlers;

public class GetJWTHandler : IRequestHandler<GetJWTQuery, ErrorOr<string>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetJWTHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ErrorOr<string>> Handle(GetJWTQuery request, CancellationToken cancellationToken)
    {
        var jwt = _httpContextAccessor.HttpContext?.Request.Headers[HTTPHeaders.Authorization].ToString().Split(' ')[1];
        var handler = new JwtSecurityTokenHandler();

        if (string.IsNullOrEmpty(jwt) || !handler.CanReadToken(jwt))
        {
            return Errors.Authorization.Unauthorized;
        }

        return await Task.FromResult(jwt);
    }
}
