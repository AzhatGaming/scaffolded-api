using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using ScaffoldedApi.Interfaces;
using ScaffoldedApi.Swagger;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace ScaffoldedApi
{
    public class Config
    {
        private Type _defaultDbContext = null!;
        private Dictionary<Type, Type> _overrideDbContexts = [];

        public Type DefaultDbContext { get => _defaultDbContext; }
        public Dictionary<Type, Type> OverrideDbContexts { get => _overrideDbContexts; }

        public void UseDbContext<T>() where T : DbContext
        {
            UseDbContext(typeof(T));
        }

        public void UseDbContext(Type dbContext)
        {
            _defaultDbContext = dbContext;
        }

        public void UseDbContextFor<T1, T2>() 
            where T1 : class 
            where T2 : DbContext
        {
            UseDbContextFor(typeof(T1), typeof(T2));
        }

        public void UseDbContextFor(Type model, Type dbContext)
        {
            _overrideDbContexts.Add(model, dbContext);
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiScaffolding(this IServiceCollection services, Action<Config>? method = null)
        {
            var options = new Config();

            if (method is not null)
            {
                method(options);
            }

            services.AddScoped(typeof(IScaffoldedRepository<>), typeof(ScaffoldedRepository<>));
            services.AddScoped(typeof(IScaffoldedDataService<>), typeof(ScaffoldedDataService<>));

            // DbContextFactories
            //services.AddSingleton<IDbContextFactory>(svc => new DbContextFactory(svc, options.DefaultDbContext));
            //services.AddScoped(typeof(IDbContextFactory<>), );

            //foreach (var context in options.OverrideDbContexts)
            //{
            //    services.AddScoped(typeof(IDbContextFactory<>).MakeGenericType(context.Key), svc => 
            //        Activator.CreateInstance(typeof(DbContextFactory<>).MakeGenericType(context.Key), svc, context.Value)!);
            //}

            services.AddScoped<Func<Type, DbContext>>(serviceProvider => key =>
            {
                if (options.OverrideDbContexts.TryGetValue(key, out Type? value))
                {
                    return (DbContext)serviceProvider.GetService(value)!;
                }

                return (DbContext)serviceProvider.GetService(options.DefaultDbContext)!;
            });

            return services;
        }

        public static void ScaffoldedApiQueryFilter(this SwaggerGenOptions options)
        {
            options.OperationFilter<AddFilterOperationFilter>();
        }
    }
}
