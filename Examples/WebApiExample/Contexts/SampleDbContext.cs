using Microsoft.EntityFrameworkCore;
using WebApiExample.Models;

namespace WebApiExample.Contexts
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<City> Cities { get; set; }
    }
}
