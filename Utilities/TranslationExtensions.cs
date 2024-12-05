using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ScaffoldedApi.Utilities
{
    public static class TranslationExtensions
    {
        public static object[] ToKeyValues<T>(this string str) where T : class
        {
            var keys = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Any());
            var items = str.Split(',');

            return keys.Select<PropertyInfo, object>((key, index) => key.PropertyType switch
            {
                Type t when t == typeof(int) => Convert.ToInt32(items[index]),
                Type t when t == typeof(Guid) => Guid.Parse(items[index]),
                _ => items[index]
            }).ToArray();
        }
    }
}
