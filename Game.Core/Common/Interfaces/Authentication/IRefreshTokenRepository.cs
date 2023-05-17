using Game.Domain.Entities;

namespace Game.Core.Common.Interfaces.Authentication;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> Get(Guid userId);
    Task Post(RefreshToken refreshToken);
    void Update(RefreshToken refreshToken);
    void Delete(RefreshToken refreshToken);
}
