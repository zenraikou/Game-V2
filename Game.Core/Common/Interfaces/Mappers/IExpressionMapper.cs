using System.Linq.Expressions;

namespace Game.Core.Common.Interfaces.Mappers;

public interface IExpressionMapper
{
    Expression<Func<TDestination, bool>> MapExpression<TSource, TDestination>(Expression<Func<TSource, bool>> expression);
}
