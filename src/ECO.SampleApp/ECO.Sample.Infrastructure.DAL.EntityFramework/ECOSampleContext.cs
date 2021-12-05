﻿using ECO.Providers.EntityFramework;
using ECO.Sample.Domain;
using ECO.Sample.Infrastructure.DAL.EntityFramework.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace ECO.Sample.Infrastructure.DAL.EntityFramework
{
    public class ECOSampleContext : ECODbContext
    {
        public DbSet<Session> Sessions { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Speaker> Speakers { get; set; }

        public ECOSampleContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EventConfiguration());
        }
    }
}
