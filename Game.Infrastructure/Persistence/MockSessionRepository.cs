using System.Linq.Expressions;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;

namespace Game.Infrastructure.Persistence;

public class MockSessionRepository : ISessionRepository
{
    public static List<Session> sessions = new();

    public async Task<IEnumerable<Session>> GetAll(Expression<Func<Session, bool>>? expression)
    {
        // change static users to DbSet<User> when using EF Core
        IQueryable<Session> query = sessions.AsQueryable();

        if (expression is not null)
        {
            query = query.Where(expression);
        }

        return await Task.FromResult(query/*.AsNoTracking()*/.AsEnumerable());
    }

    public async Task<Session?> Get(Expression<Func<Session, bool>> expression)
    {
        IQueryable<Session> query = sessions.AsQueryable();
        return await Task.FromResult(query/*AsNoTracking()*/.FirstOrDefault(expression));
    }

    public async Task Post(Session session)
    {
        sessions.Add(session);
        await Task.CompletedTask;
    }

    public void Update(Session session)
    {
        var entity = sessions.FirstOrDefault(s => s.JTI == session.JTI);

        if (entity is not null)
        {
            entity.JTI = session.JTI;
            entity.Fingerprint = session.Fingerprint; 
            entity.Expiry = session.Expiry;
        }
    }

    public void Delete(Session session)
    {
        sessions.Remove(session);
    }
}
