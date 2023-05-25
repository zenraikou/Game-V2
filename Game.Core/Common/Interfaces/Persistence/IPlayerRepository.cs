using System.Linq.Expressions;
using Game.Domain.Entities;

namespace Game.Core.Common.Interfaces.Persistence;

public interface IPlayerRepository
{
    Task<IEnumerable<Player>> GetAll(Expression<Func<Player, bool>>? expression = null);
    Task<Player?> Get(Expression<Func<Player, bool>> expression);
    Task Post(Player players);
    void Update(Player players);
    void Delete(Player players);
}
