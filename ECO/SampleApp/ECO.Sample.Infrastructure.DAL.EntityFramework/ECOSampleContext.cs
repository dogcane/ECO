using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO.Providers.EntityFramework;
using ECO.Sample.Domain;

namespace ECO.Sample.Infrastructure.DAL.EntityFramework
{
    public class ECOSampleContext : ECODbContext
    {
        public DbSet<Session> Sessions { get; set; }
        
        public DbSet<Event> Events { get; set; }

        public ECOSampleContext(string connectionString)
            : base(connectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ECO_EF");
            base.OnModelCreating(modelBuilder);            
        }
    }
}
