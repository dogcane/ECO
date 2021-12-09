using ECO.Sample.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Sample.Infrastructure.DAL.EntityFramework.EntityConfiguration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");            
            builder.HasKey(x => x.Identity);
            builder.Property(x => x.Identity).HasColumnName("Id");
            builder.Property(x => x.Name);
            builder.Property(x => x.Description);
            builder.OwnsOne(x => x.Period, p =>
            {
                p.Property(p => p.StartDate).HasColumnName("StartDate");
                p.Property(p => p.EndDate).HasColumnName("EndDate");
            });
            builder.Navigation(x => x.Sessions).AutoInclude(true);
            builder.HasMany(x => x.Sessions).WithOne(x => x.Event).HasForeignKey("FK_Event");            
        }        
    }
}
