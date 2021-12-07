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
    public class SpeakerConfiguration : IEntityTypeConfiguration<Speaker>
    {
        public void Configure(EntityTypeBuilder<Speaker> builder)
        {
            builder.ToTable("Speakers");
            builder.HasKey(x => x.Identity);
            builder.Property(x => x.Identity).HasColumnName("Id");
            builder.Property(x => x.Name);
            builder.Property(x => x.Surname);
            builder.Property(x => x.Description);
            builder.Property(x => x.Age);
        }        
    }
}
