using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Game.Core.Common.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;

namespace Game.Infrastructure.Authentication;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserClaim(Func<Claim, bool> expression)
    {
        // Get the token from the Authorization header
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token); // Read the token
        var claim = jwt.Claims.FirstOrDefault(expression)?.Value; // Extract the custom claim
        return claim;
    }
}
