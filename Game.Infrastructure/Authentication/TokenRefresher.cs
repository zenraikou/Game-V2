using System.Security.Cryptography;
using Game.Core.Common.Interfaces.Authentication;
using Game.Core.Common.Interfaces.Services;
using Game.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Game.Infrastructure.Authentication;

public class TokenRefresher : ITokenRefresher
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDateTImeProvider _dateTimeProvider;

    public TokenRefresher(IHttpContextAccessor httpContextAccessor, IDateTImeProvider dateTimeProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _dateTimeProvider = dateTimeProvider;
    }

    public RefreshToken RefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expiry = _dateTimeProvider.Now.AddDays(7)
        };

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = refreshToken.Expiry
        };

        _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

        return refreshToken;
    }
}
