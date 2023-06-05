using System.IdentityModel.Tokens.Jwt;

namespace Game.Core.Common.JWT;

public static class JWTClaims
{
    public const string JTI = "jti";
    public const string Id = "id";
    public const string Role = "role";
    public const string Expiry = JwtRegisteredClaimNames.Exp;
}
