using System.Linq.Expressions;
using System.Reflection;
using Game.Core.Common.Interfaces.ExpressionMapper;

namespace Game.Core.Mappings;

public class ExpressionMapper : IExpressionMapper
{
    public Expression<Func<TDestination, bool>> MapExpression<TSource, TDestination>(Expression<Func<TSource, bool>> expression)
    {
        var parameter = Expression.Parameter(typeof(TDestination), "destination");
        var visitor = new ReplaceVisitor<TDestination>(expression.Parameters[0], parameter);
        var mappedExpressionBody = visitor.Visit(expression.Body);

        var mappedExpression = Expression.Lambda<Func<TDestination, bool>>(mappedExpressionBody, parameter);

        return mappedExpression;
    }

    private class ReplaceVisitor<TDestination> : ExpressionVisitor
    {
        private readonly ParameterExpression _sourceParameter;
        private readonly ParameterExpression _destinationParameter;

        public ReplaceVisitor(ParameterExpression sourceParameter, ParameterExpression destinationParameter)
        {
            _sourceParameter = sourceParameter;
            _destinationParameter = destinationParameter;
        }

        // protected override Expression VisitParameter(ParameterExpression node)
        // {
        //     return _destinationParameter;
        // }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression == _sourceParameter)
            {
                var memberInfo = GetMemberInfo<TDestination>(node.Member.Name);
                var memberExpression = Expression.MakeMemberAccess(_destinationParameter, memberInfo);
                return memberExpression;
            }

            return base.VisitMember(node);
        }

        private MemberInfo GetMemberInfo<T>(string memberName)
        {
            var memberInfo = typeof(T).GetMember(memberName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            return memberInfo[0];
        }
    }
}
