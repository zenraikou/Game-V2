using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Game.Core.TempServices.UserClaim;
using Microsoft.AspNetCore.Http;

namespace Game.Infrastructure.Authentication;

public class UserClaimService : IUserClaimService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserClaimService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserClaim(Func<Claim, bool> expression)
    {
        // Get the jwt from the Authorization header
        var jwt = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt); // Read the jwt
        var claim = token.Claims.FirstOrDefault(expression)?.Value; // Extract the custom claim
        return claim;
    }
}
