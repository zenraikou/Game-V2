using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;
using Game.Infrastructure.Database;

namespace Game.Infrastructure.Persistence;

public class SessionRepository : GenericRepository<Session>, ISessionRepository
{
    public SessionRepository(GameDBContext context) : base(context) { }
}
