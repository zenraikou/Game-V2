using System.Linq.Expressions;

namespace Game.Core.Common.Interfaces.Persistence;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? expression = null);
    Task<T?> Get(Expression<Func<T, bool>> expression);
    Task Post(T entity);
    Task Update(T entity);
    Task Delete(T entity);
}
