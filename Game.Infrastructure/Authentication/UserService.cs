using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Game.Core.Common.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;

namespace Game.Core.Services.User;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserUniqueName()
    {
        var result = string.Empty;

        if (_httpContextAccessor.HttpContext is not null)
        {
            result = _httpContextAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.UniqueName);
        }

        return result;
    }
}
