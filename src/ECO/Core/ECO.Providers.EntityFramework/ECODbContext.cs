using Microsoft.EntityFrameworkCore;

namespace ECO.Providers.EntityFramework
{
    public abstract class ECODbContext : DbContext
    {
        protected ECODbContext(DbContextOptions options)
            : base(options)
        {

        }

    }
}
