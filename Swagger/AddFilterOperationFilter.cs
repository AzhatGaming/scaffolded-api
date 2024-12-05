using Microsoft.OpenApi.Models;
using ScaffoldedApi.QueryFilter;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ScaffoldedApi.Swagger
{
    public class AddFilterOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filter = context.MethodInfo.GetCustomAttributes(true).OfType<QueryFilterAttribute>().FirstOrDefault();
            if (filter is not null)
            {
                var type = context.MethodInfo.DeclaringType?.GenericTypeArguments[0];
                if (type is null) return;

                var parameters = type?.GetProperties().Where(p => p.GetCustomAttributes(true).OfType<FilterAttribute>().Any())
                    .Select(p =>
                    {
                        var attr = p.GetCustomAttributes(true).OfType<FilterAttribute>().First();

                        return new OpenApiParameter
                        {
                            Name = p.Name,
                            In = ParameterLocation.Query,
                            Required = false,
                            Schema = new OpenApiSchema
                            {
                                Type = attr.Name
                            },
                            Description = attr.Description,
                        };
                    });

                var genericParameters = type?.GetProperties().Where(p => p.GetCustomAttributes(true).Any(a => a.GetType().IsGenericType && a.GetType().GetGenericTypeDefinition() == typeof(FilterAttribute<>)))
                    .Select(p =>
                    {
                        var attr = (FilterAttribute)p.GetCustomAttributes(true).First(a => a.GetType().IsGenericType && a.GetType().GetGenericTypeDefinition() == typeof(FilterAttribute<>));

                        return new OpenApiParameter
                        {
                            Name = p.Name,
                            In = ParameterLocation.Query,
                            Required = false,
                            Schema = new OpenApiSchema
                            {
                                Type = attr.Name
                            },
                            Description = attr.Description,
                        };
                    });

                if (operation.Parameters is null)
                {
                    operation.Parameters = [];
                }

                foreach (var parameter in parameters!)
                {
                    operation.Parameters.Add(parameter);
                }

                foreach (var parameter in genericParameters!)
                {
                    operation.Parameters.Add(parameter);
                }
            }
        }
    }
}
