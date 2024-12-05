using System.Linq.Expressions;

namespace ScaffoldedApi.QueryFilter
{
    public class StringQueryFilter : IQueryFilter
    {
        public Expression<Func<T, bool>> GetFilterExpression<T>(string propertyName, object value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyExpression = Expression.Property(parameter, propertyName);

            var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
            var lowerProperty = Expression.Call(propertyExpression, toLowerMethod!);

            var constant = Expression.Constant(value.ToString()!.ToLower());
            var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)]);

            var expression = Expression.Call(lowerProperty, containsMethod!, constant);
            return Expression.Lambda<Func<T, bool>>(expression, parameter);
        }
    }
}
