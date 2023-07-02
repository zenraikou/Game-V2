using ErrorOr;
using Game.Core.Common.Constants;
using Game.Domain.Common.Errors;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Game.Core.Services.Authentications.Queries.GetJWT;

public class GetJWTHandler : IRequestHandler<GetJWTQuery, ErrorOr<string>>
{
    private readonly IHttpContextAccessor _accessor;

    public GetJWTHandler(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public async Task<ErrorOr<string>> Handle(GetJWTQuery request, CancellationToken cancellationToken)
    {
        var jwt = _accessor.HttpContext?.Request.Headers[HTTPHeaders.Authorization].ToString().Split(' ')[1];
        var handler = new JwtSecurityTokenHandler();

        if (string.IsNullOrEmpty(jwt) || !handler.CanReadToken(jwt))
        {
            return Errors.Authorization.Unauthorized;
        }

        return await Task.FromResult(jwt);
    }
}
