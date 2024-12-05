using System.Linq.Expressions;

namespace ScaffoldedApi.QueryFilter
{
    public interface IQueryFilter
    {
        public Expression<Func<T, bool>> GetFilterExpression<T>(string propertyName, object value);
    }
}
