using System.Linq.Expressions;
using Game.Domain.Entities;

namespace Game.Core.Common.Interfaces.Persistence;

public interface IRefreshTokenRepository
{
    Task<IEnumerable<RefreshToken>> GetAll(Expression<Func<RefreshToken, bool>>? expression = null);
    Task<RefreshToken?> Get(Expression<Func<RefreshToken, bool>> expression);
    Task Post(RefreshToken refreshToken);
    void Update(RefreshToken refreshToken);
    void Delete(RefreshToken refreshToken);
}
