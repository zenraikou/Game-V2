using Game.Core.Common.Interfaces.Persistence;
using Game.Infrastructure.Database;

namespace Game.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly GameDBContext _context;
    public IPlayerRepository Players { get; private set; }
    public ISessionRepository Sessions { get; private set; }

    public UnitOfWork(GameDBContext context)
    {
        _context = context;
        Players = new PlayerRepository(context);
        Sessions = new SessionRepository(context);
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}
