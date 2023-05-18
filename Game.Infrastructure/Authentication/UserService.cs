using System.IdentityModel.Tokens.Jwt;
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

    public Guid? GetUserIdClaims()
    {
        // Get the token from the Authorization header
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token); // Read the token
        var id = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value; // Extract the custom claim
        return id is null ? null : Guid.Parse(id);
    }
}
