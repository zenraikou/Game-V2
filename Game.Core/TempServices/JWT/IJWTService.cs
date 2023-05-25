using Game.Domain.Entities;

namespace Game.Core.TempServices.JWT;

public interface IJWTService
{
    string GenerateJWT(Player player);
    Session GenerateSession(string jwt);
}
