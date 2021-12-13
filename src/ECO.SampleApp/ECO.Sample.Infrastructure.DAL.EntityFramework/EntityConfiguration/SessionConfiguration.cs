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
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Sessions");
            builder.HasKey(x => x.Identity);            
            builder.Property(x => x.Identity).HasColumnName("Id");
            builder.Property(x => x.Title);
            builder.Property(x => x.Description);
            builder.Property(x => x.Level);
            builder.Navigation(x => x.Event).AutoInclude(true);
            builder.HasOne(x => x.Event).WithMany(x => x.Sessions).HasForeignKey("FK_Event");
            builder.Navigation(x => x.Speaker).AutoInclude(true);
            builder.HasOne(x => x.Speaker).WithMany().HasForeignKey("FK_Speaker");
        }        
    }
}
