using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using Game.Infrastructure.Database;

namespace Game.Infrastructure.Persistence;

public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
{
    public PlayerRepository(GameDBContext context) : base(context) { }
}
