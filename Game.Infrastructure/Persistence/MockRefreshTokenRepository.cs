using System.Linq.Expressions;
using Game.Core.Common.Interfaces.Authentication;
using Game.Domain.Entities;

namespace Game.Infrastructure.Persistence;

public class MockRefreshTokenRepository : IRefreshTokenRepository
{
    public static List<RefreshToken> refreshTokens = new();

    // get
    public async Task<RefreshToken> Get(Guid userId)
    {
        var refreshToken = refreshTokens.FirstOrDefault(t => t.UserId == userId)!;
        return await Task.FromResult(refreshToken);
    }
    // add
    public async Task Post(RefreshToken refreshToken)
    {
        refreshTokens.Add(refreshToken);
        await Task.CompletedTask;
    }
    // update
    public void Update(RefreshToken refreshToken)
    {
        var entity = refreshTokens.FirstOrDefault(t => t.UserId == refreshToken.UserId);

        if (entity is not null)
        {
            entity.Token = refreshToken.Token; 
            entity.Expiry = refreshToken.Expiry;
        }
    }

    public void Delete(RefreshToken refreshToken)
    {
        refreshTokens.Remove(refreshToken);
    }
}
