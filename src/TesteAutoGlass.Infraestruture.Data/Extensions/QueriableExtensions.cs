using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using TesteAutoGlass.Utils.Abstractions.Entities.Entities;
using TesteAutoGlass.Utils.Abstractions.Pagination.Options;

namespace TesteAutoGlass.Infraestruture.Data.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class QueriableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source
                                               , SortOptionsDto sortOptions)
            where T : EntityBase
        {
            if (sortOptions == null)
            {
                return source;
            }

            var expression = source.Expression;
            var selector = PrepareSelector<T>(sortOptions.SortColumns[0], out var parameter);

            var method = sortOptions.Ascending ? "OrderBy" : "OrderByDescending";

            expression = Expression.Call(typeof(Queryable)
                                         , method
                                         , new[] { source.ElementType, selector.Type }
                                         , expression
                                         , Expression.Quote(Expression.Lambda(selector, parameter)));

            if (sortOptions.SortColumns.Length > 1)
            {
                for (var i = 1; i < sortOptions.SortColumns.Length; i++)
                {
                    expression = ThenBy(source, sortOptions.SortColumns[i], sortOptions, expression);
                }
            }

            return source.Provider.CreateQuery<T>(expression);
        }

        public static MethodCallExpression ThenBy<T>(this IQueryable<T> source
                                                     , string sortProperty
                                                     , SortOptionsDto sortOrder
                                                     , Expression expression)
            where T : EntityBase
        {
            var selector = PrepareSelector<T>(sortProperty, out var parameter);
            var orderByExp = Expression.Lambda(selector, parameter);
            var typeArguments = new[] { source.ElementType, selector.Type };
            var methodName = sortOrder.Ascending ? "ThenBy" : "ThenByDescending";

            var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, expression, Expression.Quote(orderByExp));

            return resultExp;
        }

        private static Expression PrepareSelector<T>(string selectorName
                                                     , out ParameterExpression parameterExpression)
            where T : EntityBase
        {
            parameterExpression = Expression.Parameter(typeof(T), "x");
            return selectorName.Split('.')
                               .Aggregate<string, Expression>(parameterExpression, Expression.PropertyOrField);
        }
    }
}
