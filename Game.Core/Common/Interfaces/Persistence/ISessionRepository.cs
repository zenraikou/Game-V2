using System.Linq.Expressions;
using Game.Domain.Entities;

namespace Game.Core.Common.Interfaces.Persistence;

public interface ISessionRepository
{
    Task<IEnumerable<Session>> GetAll(Expression<Func<Session, bool>>? expression = null);
    Task<Session?> Get(Expression<Func<Session, bool>> expression);
    Task Post(Session session);
    void Update(Session session);
    void Delete(Session session);
}
