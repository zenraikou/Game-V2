using System.Linq.Expressions;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;

namespace Game.Infrastructure.Persistence;

public class MockUserRepository : IUserRepository
{
    public static List<User> users = new();

    public async Task<IEnumerable<User>> GetAll(Expression<Func<User, bool>> expression)
    {
        var query = users.AsQueryable().Where(expression);
        return await Task.FromResult(users.AsEnumerable());
    }

    public async Task<User?> Get(Func<User, bool> expression)
    {
        return await Task.FromResult(users.FirstOrDefault(expression));
    }

    public async Task Post(User user)
    {
        users.Add(user);
        await Task.CompletedTask;
    }

    public void Update(User user)
    {
        var entity = users.FirstOrDefault(u => u.Id == user.Id);

        if (entity is not null)
        {
            entity.Handle = user.Handle;
            entity.Name = user.Name;
            entity.UniqueName = user.UniqueName;
            entity.Email = user.Email;
            entity.PasswordHash = user.PasswordHash;
            entity.Role = user.Role;
        }
    }

    public void Delete(Guid id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user is null) throw new Exception("Failed to delete user.");
        users.Remove(user);
    }
}
