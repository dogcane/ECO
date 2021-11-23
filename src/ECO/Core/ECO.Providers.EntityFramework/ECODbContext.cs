using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ECO.Providers.EntityFramework
{
    public abstract class ECODbContext : DbContext
    {
        protected ECODbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach(var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.ClrType).HasKey("Identity");
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
