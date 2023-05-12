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

    public List<string> GetCurrentUserUniqueName()
    {
        var result = new List<string>();

        // Get the token from the Authorization header
        var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        var token = authHeader?.Replace("Bearer ", "");

        // Create a JwtSecurityTokenHandler to read the token
        var tokenHandler = new JwtSecurityTokenHandler();

        // Read the token and extract the custom claim
        var jwtToken = tokenHandler.ReadJwtToken(token);

        var sub = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        var name = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
        var uniqueName = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value;
        var email = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
        var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

        if (sub is null) throw new Exception($"Sub is null.");
        if (name is null) throw new Exception($"Name is null.");
        if (uniqueName is null) throw new Exception($"Unique Name is null.");
        if (email is null) throw new Exception($"Email is null.");
        if (jti is null) throw new Exception($"JTI is null.");
        if (role is null) throw new Exception($"Role is null.");

        result.Add(sub);
        result.Add(name);
        result.Add(uniqueName);
        result.Add(email);
        result.Add(jti);
        result.Add(role);

        return result;
    }
}
