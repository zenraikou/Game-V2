using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;

namespace Game.Infrastructure.Persistence;

public class MockUserRepository : IUserRepository
{
    public static List<User> users = new();

    public Task<List<User>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<User> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task Post(User user)
    {
        throw new NotImplementedException();
    }

    public void Update(User user)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}
