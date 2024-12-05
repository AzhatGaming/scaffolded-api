using Microsoft.EntityFrameworkCore;

namespace ScaffoldedApi.Interfaces
{
    public interface IDbContextFactory<T> : IDbContextFactory where T : class
    {

    }

    public interface IDbContextFactory
    {
        public DbContext CreateDbContext();
    }
}
