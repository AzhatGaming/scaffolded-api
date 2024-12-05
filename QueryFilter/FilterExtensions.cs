using System.Linq.Expressions;
using System.Reflection;

namespace ScaffoldedApi.QueryFilter
{
    public static class FilterExtensions
    {
        public static IEnumerable<Expression<Func<T, bool>>> GetQueryFilters<T>(this IDictionary<string, object> queryParameters) where T : class
        {
            var expressions = new List<Expression<Func<T, bool>>>();

            if (queryParameters is not null)
            {
                foreach (var (property, attribute) in GetFilterAttributes<T>())
                {
                    if (!queryParameters.ContainsKey(property.Name))
                    {
                        continue;
                    }

                    var value = queryParameters[property.Name];
                    if (ShouldCreateExpression(property, value))
                    {
                        expressions.Add(attribute.Filter.GetFilterExpression<T>(property.Name, value));
                    }
                }
            }

            return expressions;
        }

        private static IEnumerable<(PropertyInfo Property, FilterAttribute Attribute)> GetFilterAttributes<T>()
        {
            return typeof(T).GetProperties()
                .Select(p => (Property: p, Attribute: p.GetCustomAttributes(true).OfType<FilterAttribute>().FirstOrDefault()!))
                .Where(p => p.Attribute is not null);
        }

        private static bool ShouldCreateExpression(PropertyInfo property, object? value)
        {
            var defaultValue = property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null;
            return !Equals(value, defaultValue);
        }
    }
}
