using Game.Domain.Entities;

namespace Game.Core.Common.Interfaces.Authentication;

public interface ITokenService
{
    string GenerateJWT(User user);
    RefreshToken GenerateRefreshToken();
    void RefreshToken(Guid id, RefreshToken refreshToken);
}
