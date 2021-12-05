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
            var period = builder.OwnsOne(x => x.Period);
            period.Property(p => p.StartDate);
            period.Property(p => p.EndDate);
            builder.Ignore(x => x.Sessions);
            //builder.OwnsMany(x => x.Sessions);
        }        
    }
}
