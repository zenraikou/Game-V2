using System.Linq.Expressions;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;

namespace Game.Infrastructure.Persistence;

public class MockPlayerRepository : IPlayerRepository
{
    public static List<Player> players = new();

    public async Task<IEnumerable<Player>> GetAll(Expression<Func<Player, bool>>? expression)
    {
        // change static users to DbSet<Player> when using EF Core
        IQueryable<Player> query = players.AsQueryable();

        if (expression is not null)
        {
            query = query.Where(expression);
        }

        return await Task.FromResult(query/*.AsNoTracking()*/.AsEnumerable());
    }

    public async Task<Player?> Get(Expression<Func<Player, bool>> expression)
    {
        // change static users to DbSet<Player> when using EF Core
        IQueryable<Player> query = players.AsQueryable();
        return await Task.FromResult(query/*.AsNoTracking()*/.FirstOrDefault(expression));
    }

    public async Task Post(Player player)
    {
        players.Add(player);
        await Task.CompletedTask;
    }

    public void Update(Player player)
    {
        var entity = players.FirstOrDefault(u => u.Id == player.Id);

        if (entity is not null)
        {
            entity.Handle = player.Handle;
            entity.Name = player.Name;
            entity.UniqueName = player.UniqueName;
            entity.Email = player.Email;
            entity.PasswordHash = player.PasswordHash;
            entity.Role = player.Role;
        }
    }

    public void Delete(Player player)
    {
        players.Remove(player);
    }
}
