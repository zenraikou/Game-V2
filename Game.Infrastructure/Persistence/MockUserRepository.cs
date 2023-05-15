using System.Linq.Expressions;
using Game.Core.Common.Interfaces.Persistence;
using Game.Domain.Entities;

namespace Game.Infrastructure.Persistence;

public class MockUserRepository : IUserRepository
{
    public static List<User> users = new();

    public async Task<IEnumerable<User>> GetAll(Expression<Func<User, bool>>? expression)
    {
        // change static users to DbSet<User> when using EF Core
        IQueryable<User> query = users.AsQueryable();

        if (expression is not null)
        {
            query = query.Where(expression);
        }

        return await Task.FromResult(query/*.AsNoTracking()*/.AsEnumerable());
    }

    public async Task<User?> Get(Expression<Func<User, bool>> expression)
    {
        // change static users to DbSet<User> when using EF Core
        IQueryable<User> query = users.AsQueryable();
        return await Task.FromResult(query/*.AsNoTracking()*/.FirstOrDefault(expression));
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

    public void Delete(User user)
    {
        users.Remove(user);
    }
}
