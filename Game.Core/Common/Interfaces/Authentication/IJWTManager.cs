using Game.Domain.Entities;

namespace Game.Core.Common.Interfaces.Authentication;

public interface IJWTManager
{
    string GenerateToken(User user);
    RefreshToken GenerateRefreshToken();
    void SetRefreshToken(Guid id, RefreshToken refreshToken);
}
