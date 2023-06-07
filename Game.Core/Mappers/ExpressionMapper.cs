using Game.Core.Common.Interfaces.Mappers;
using System.Linq.Expressions;
using System.Reflection;

namespace Game.Core.Mappers;

public class ExpressionMapper : IExpressionMapper
{
    public Expression<Func<TDestination, bool>> MapExpression<TSource, TDestination>(Expression<Func<TSource, bool>> expression)
    {
        var destinationParameter = Expression.Parameter(typeof(TDestination), "destination");
        var visitor = new ReplaceVisitor<TDestination>(expression.Parameters[0], destinationParameter);
        var mappedExpressionBody = visitor.Visit(expression.Body);
        var mappedExpression = Expression.Lambda<Func<TDestination, bool>>(mappedExpressionBody, destinationParameter);
        return mappedExpression;
    }
}

public class ReplaceVisitor<TDestination> : ExpressionVisitor
{
    private readonly ParameterExpression _sourceParameter;
    private readonly ParameterExpression _destinationParameter;

    public ReplaceVisitor(ParameterExpression sourceParameter, ParameterExpression destinationParameter)
    {
        _sourceParameter = sourceParameter;
        _destinationParameter = destinationParameter;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Expression == _sourceParameter)
        {
            var memberInfo = GetDestinationMember(node.Member.Name);
            var memberExpression = Expression.MakeMemberAccess(_destinationParameter, memberInfo);
            return memberExpression;
        }

        return base.VisitMember(node);
    }

    private MemberInfo GetDestinationMember(string memberName)
    {
        var destinationType = typeof(TDestination);
        var memberInfo = destinationType.GetMember(memberName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        return memberInfo[0];
    }
}
