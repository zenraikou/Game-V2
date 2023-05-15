using Game.Domain.Entities;

namespace Game.Core.Common.Interfaces.Persistence;

public interface IUserRepository
{
    Task<List<User>> GetAll();
    Task<User> Get(Guid id);
    Task Post(User user);
    void Update(User user);
    void Delete(Guid id);
}
