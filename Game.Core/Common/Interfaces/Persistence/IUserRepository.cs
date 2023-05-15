using System.Linq.Expressions;
using Game.Domain.Entities;

namespace Game.Core.Common.Interfaces.Persistence;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll(Expression<Func<User, bool>> expression);
    Task<User?> Get(Func<User, bool> expression);
    Task Post(User user);
    void Update(User user);
    void Delete(Guid id);
}
