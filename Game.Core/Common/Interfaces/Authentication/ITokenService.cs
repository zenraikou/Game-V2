using Game.Domain.Entities;

namespace Game.Core.Common.Interfaces.Authentication;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken(Guid userId);
}
