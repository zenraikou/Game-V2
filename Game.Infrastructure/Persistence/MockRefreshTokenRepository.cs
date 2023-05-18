using System.Linq.Expressions;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;

namespace Game.Infrastructure.Persistence;

public class MockRefreshTokenRepository : IRefreshTokenRepository
{
    public static List<RefreshToken> refreshTokens = new();

    public async Task<IEnumerable<RefreshToken>> GetAll(Expression<Func<RefreshToken, bool>>? expression)
    {
        // change static users to DbSet<User> when using EF Core
        IQueryable<RefreshToken> query = refreshTokens.AsQueryable();

        if (expression is not null)
        {
            query = query.Where(expression);
        }

        return await Task.FromResult(query/*.AsNoTracking()*/.AsEnumerable());
    }

    public async Task<RefreshToken?> Get(Expression<Func<RefreshToken, bool>> expression)
    {
        IQueryable<RefreshToken> query = refreshTokens.AsQueryable();
        return await Task.FromResult(query/*AsNoTracking()*/.FirstOrDefault(expression));
    }

    public async Task Post(RefreshToken refreshToken)
    {
        refreshTokens.Add(refreshToken);
        await Task.CompletedTask;
    }

    public void Update(RefreshToken refreshToken)
    {
        var entity = refreshTokens.FirstOrDefault(t => t.UserId == refreshToken.UserId);

        if (entity is not null)
        {
            entity.Value = refreshToken.Value; 
            entity.Expiry = refreshToken.Expiry;
        }
    }

    public void Delete(RefreshToken refreshToken)
    {
        refreshTokens.Remove(refreshToken);
    }
}
