using System.Linq.Expressions;

namespace Game.Core.Common.Interfaces.ExpressionMapper;

public interface IExpressionMapper
{
    Expression<Func<TDestination, bool>> MapExpression<TSource, TDestination>(Expression<Func<TSource, bool>> expression);
}
