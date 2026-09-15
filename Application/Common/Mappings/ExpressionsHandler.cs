using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CleanArchitecture.Application.Common.Mappings;
using Common.Utilities;

namespace DataTransferObjects.CustomExpressions
{
    public static class ExpressionsHelper
    {
        //public static List<Expression<Func<TEntity, bool>>> GenerateActorsExpression<TEntity, TDto, TKey>(TDto dto)
        //    where TDto : BaseSearchDto
        //    where TEntity : BaseEntityWithActors<TKey>
        //{
        //    List<Expression<Func<TEntity, bool>>> expressions = new List<Expression<Func<TEntity, bool>>>
        //    {
        //    };

        //    if (dto.IsActive is null)
        //        expressions.Add(src => src.IsActive == true);

        //    if (dto.IsActive is not null)
        //        expressions.Add(src => src.IsActive == dto.IsActive);

        //    if (dto.Id is not null && (long)(object)dto.Id > 0)
        //        expressions.Add(src => (long)(object)src.Id == (long)(object)dto.Id);

        //    if (dto.CreatorId > 0)
        //        expressions.Add(src => src.CreatorId == dto.CreatorId);

        //    if (dto.ModifierId > 0)
        //        expressions.Add(src => src.ModifierId == dto.ModifierId);

        //    if (!String.IsNullOrEmpty(dto.FromCreationDate))
        //        expressions.Add(src => src.CreationDate.Date >= PersianDateExtensions.ToGregorianDate((string) dto.FromCreationDate));

        //    if (!String.IsNullOrEmpty(dto.ToCreationDate))
        //        expressions.Add(src => src.CreationDate.Date <= PersianDateExtensions.ToGregorianDate((string) dto.ToCreationDate));

        //    if (!String.IsNullOrEmpty(dto.FromModificationDate))
        //        expressions.Add(src => src.ModificationDate.Date >= PersianDateExtensions.ToGregorianDate((string) dto.FromModificationDate));

        //    if (!String.IsNullOrEmpty(dto.ToModificationDate))
        //        expressions.Add(src => src.ModificationDate.Date <= PersianDateExtensions.ToGregorianDate((string) dto.ToModificationDate));

        //    return expressions;

        //}

        //private static Expression<Func<TEntity, bool>> Combine<TEntity>(IList<Expression<Func<TEntity, bool>>> expressions)
        //{
        //    if (expressions.Count == 0)
        //    {
        //        return null;
        //    }

        //    Expression<Func<TEntity, bool, bool, bool>> combiningExpr =
        //        (c, expr1, expr2) => expr1 && expr2;

        //    LambdaExpression combined = expressions[0];
        //    foreach (var expr in expressions.Skip(1))
        //    {
        //        // ReplacePar comes from the library, it's an extension
        //        // requiring `using LinqExprHelper`.
        //        combined = combiningExpr
        //            .ReplacePar("expr1", combined.Body)
        //            .ReplacePar("expr2", expr.Body);
        //    }
        //    return (Expression<Func<TEntity, bool>>)combined;
        //}

        //public static Expression<Func<T, bool>> AndAll<T>(
        //    IList<Expression<Func<T, bool>>> expressions)
        //{
        //    try
        //    {
        //        if (expressions == null)
        //        {
        //            throw new ArgumentNullException("expressions");
        //        }
        //        if (!expressions.Any())
        //        {
        //            return t => true;
        //        }
        //        Type delegateType = typeof(Func<,>)
        //            .GetGenericTypeDefinition()
        //            .MakeGenericType(new[] {
        //                    typeof(T),
        //                    typeof(bool)
        //                }
        //            );
        //        var combined = expressions
        //            .Cast<Expression>()
        //            .Aggregate((e1, e2) => Expression.AndAlso(e1, e2));
        //        return (Expression<Func<T, bool>>)Expression.Lambda(delegateType, combined);
        //    }
        //    catch (Exception ex)
        //    {
        //        return t => true;
        //    }

        //}

        public static Expression<Func<T, bool>> AndAll<T>(
            IList<Expression<Func<T, bool>>> expressions)
        {
            try
            {
                if (expressions == null)
                {
                    throw new ArgumentNullException("expressions");
                }
                if (!expressions.Any())
                {
                    return t => true;
                }

                Expression<Func<T, bool>> expression = expressions[0];

                if (expressions.Count == 1)
                    return expression;

                for (int i = 1; i < expressions.Count; i++)
                    expression = expression.AndExpression(expressions[i]);

                return expression;
            }
            catch (Exception ex)
            {
                return t => true;
            }

        }

        public static Expression<Func<T, bool>> Aliiiii<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null) return right;
            var and = Expression.AndAlso(
                left.Body, right.Body);

            //and = Expression.And(
            //    left.Body, right.Body);

            //and = Expression.AndAssign(
            //    left.Body, right.Body);
            return Expression.Lambda<Func<T, bool>>(and, left.Parameters.Single());
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null) return right;
            var and = Expression.OrElse(left.Body, right.Body);
            return Expression.Lambda<Func<T, bool>>(and, left.Parameters.Single());
        }

        public static Expression<Func<T, bool>> AndExpression<T>(this
            Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var visitor = new ParameterReplaceVisitor()
            {
                Target = right.Parameters[0],
                Replacement = left.Parameters[0],
            };

            var rewrittenRight = visitor.Visit(right.Body);
            var andExpression = Expression.AndAlso(left.Body, rewrittenRight);
            return Expression.Lambda<Func<T, bool>>(andExpression, left.Parameters);
        }
    }

    public class ParameterReplaceVisitor : ExpressionVisitor
    {
        public ParameterExpression Target { get; set; }
        public ParameterExpression Replacement { get; set; }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == Target ? Replacement : base.VisitParameter(node);
        }
    }
}
