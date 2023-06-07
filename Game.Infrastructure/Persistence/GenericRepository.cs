using Game.Core.Common.Interfaces.Persistence;
using Game.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Game.Infrastructure.Persistence;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly GameDBContext _context;
    private readonly DbSet<T> db;

    public GenericRepository(GameDBContext context)
    {
        _context = context;
        db = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? expression)
    {
        IQueryable<T> query = db.AsQueryable();

        if (expression is not null)
        {
            query = query.Where(expression);
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<T?> Get(Expression<Func<T, bool>> expression)
    {
        IQueryable<T> query = db.AsQueryable();
        return await query.AsNoTracking().FirstOrDefaultAsync(expression);
    }

    public async Task Post(T entity)
    {
        await db.AddAsync(entity);
    }

    public async Task Update(T entity)
    {
        db.Update(entity);
        await Task.CompletedTask;
    }

    public async Task Delete(T entity)
    {
        db.Remove(entity);
        await Task.CompletedTask;
    }
}
