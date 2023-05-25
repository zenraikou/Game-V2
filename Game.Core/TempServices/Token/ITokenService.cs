using Game.Domain.Entities;

namespace Game.Core.TempServices.Token;

public interface ITokenService
{
    string GenerateJWT(User user);
    Session GenerateSession(string jwt);
}
