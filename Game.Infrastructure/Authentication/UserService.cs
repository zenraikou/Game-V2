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

    public Guid? GetUserClaimsId()
    {
        // Get the token from the Authorization header
        var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        var split = authHeader?.Split(' ');

        var token = split?[1];
        var tokenHandler = new JwtSecurityTokenHandler(); // Create a JwtSecurityTokenHandler to read the token
        var jwt = tokenHandler.ReadJwtToken(token); // Read the token and extract the custom claim

        var id = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        return id is null ? null : Guid.Parse(id);
    }
}
