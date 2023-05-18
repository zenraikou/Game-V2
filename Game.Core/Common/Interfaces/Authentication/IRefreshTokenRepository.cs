using System.Linq.Expressions;
using Game.Domain.Entities;

namespace Game.Core.Common.Interfaces.Authentication;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> Get(Expression<Func<RefreshToken, bool>> expression);
    Task Post(RefreshToken refreshToken);
    void Update(RefreshToken refreshToken);
    void Delete(RefreshToken refreshToken);
}
