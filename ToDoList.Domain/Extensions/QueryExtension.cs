using System.Linq.Expressions;

namespace ToDoList.Domain.Extensions
{
    public static class QueryExtension
    {
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> values, bool flag, Expression<Func<TSource, bool>> predicate)
        {
            if (flag)
                return values.Where(predicate);
            return values;
        }
    }
}
