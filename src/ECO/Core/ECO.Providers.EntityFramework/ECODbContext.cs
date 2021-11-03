using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Providers.EntityFramework
{
    public abstract class ECODbContext : DbContext
    {
        protected ECODbContext(string connectionString)
            : base(connectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties().Where(p => p.Name == "Identity").Configure(p => p.IsKey()); 
            base.OnModelCreating(modelBuilder);
        }
    }
}
