using System.Linq.Expressions;

namespace ScaffoldedApi.QueryFilter
{
    public class DateRangeQueryFilter : IQueryFilter
    {
        public Expression<Func<T, bool>> GetFilterExpression<T>(string propertyName, object value)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyExpression = Expression.Property(parameter, propertyName);
            var range = value.ToString()!.Split('-'); // This needs some validation at some point.
            var start = DateTime.Parse(range[0]);
            var end = DateTime.Parse(range[1]);

            var startConstant = Expression.Constant(start);
            var endConstant = Expression.Constant(end);
            var startEquals = Expression.GreaterThanOrEqual(propertyExpression, startConstant);
            var endEquals = Expression.LessThanOrEqual(propertyExpression, endConstant);

            var expression = Expression.AndAlso(startEquals, endEquals);
            return Expression.Lambda<Func<T, bool>>(expression, parameter);
        }
    }
}
