# Entity Framework Core Provider

The Entity Framework Core (EF Core) provider integrates ECO with Microsoft's modern, lightweight, and cross-platform ORM. It provides excellent performance, strong typing, and comprehensive LINQ support, making it ideal for modern .NET applications.

## Features

- **Modern ORM**: Built on EF Core's latest features and performance improvements
- **Cross-Platform**: Works on .NET Core, .NET 5+, and .NET Framework
- **Code-First**: Define your model in code with migrations support
- **LINQ Support**: Full LINQ-to-SQL capabilities with IntelliSense
- **Multiple Databases**: SQL Server, PostgreSQL, MySQL, SQLite, Oracle, and more
- **Change Tracking**: Automatic change detection and state management
- **Migration Support**: Schema versioning and deployment
- **Performance**: Optimized queries and efficient data access patterns

## Installation

Install the Entity Framework provider package:

```bash
dotnet add package ECO.Providers.EntityFramework
```

```xml
<PackageReference Include="ECO.Providers.EntityFramework" Version="3.0.0" />
```

## Configuration

### Fluent Configuration

Configure Entity Framework provider using the fluent API in your startup:

#### SQL Server
~~~ c#
using ECO.Providers.EntityFramework.Configuration;

builder.Services.AddDataContext(options =>
{
    options.UseEntityFramework<YourDbContext>("ecosampleapp.efcore.sqlserver", opt => 
        opt.DbContextOptions.UseSqlServer(builder.Configuration.GetConnectionString("sqlserver")));
});

// Register repositories
builder.Services.AddScoped<IEventRepository, EventEFRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerEFRepository>();
~~~

#### PostgreSQL
~~~ c#
using ECO.Providers.EntityFramework.Configuration;

builder.Services.AddDataContext(options =>
{
    options.UseEntityFramework<YourDbContext>("ecosampleapp.efcore.postgresql", opt => 
        opt.DbContextOptions.UseNpgsql(builder.Configuration.GetConnectionString("postgres")));
});

// Register repositories
builder.Services.AddScoped<IEventRepository, EventEFRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerEFRepository>();
~~~

#### In-Memory Database
~~~ c#
using ECO.Providers.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore.Diagnostics;

builder.Services.AddDataContext(options =>
{
    options.UseEntityFramework<YourDbContext>("ecosampleapp.efcore.memory", opt =>
    {
        opt.DbContextOptions
            .UseInMemoryDatabase("ecosampleapp.efcore.memory")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
    });
});

// Register repositories
builder.Services.AddScoped<IEventRepository, EventEFRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerEFRepository>();
~~~

### Custom DbContext Setup

~~~ c#
public class ECODbContext : DbContext
{
    public ECODbContext(DbContextOptions<ECODbContext> options) : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<Speaker> Speakers { get; set; }
    public DbSet<Session> Sessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply configurations
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new SpeakerConfiguration());
        modelBuilder.ApplyConfiguration(new SessionConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}

~~~

### Repository Implementation

Create typed repositories extending the EF Core base repository:

~~~ c#
public interface IEventRepository : IRepository<Event, Guid>
{
    Task<IEnumerable<Event>> GetEventsBySpeakerAsync(Guid speakerId);
    Task<IEnumerable<Event>> GetEventsInPeriodAsync(DateTime start, DateTime end);
}

public class EventEFRepository : EntityFrameworkRepository<Event, Guid>, IEventRepository
{
    public EventEFRepository(IDataContext dataContext) 
        : base(dataContext)
    {
    }

    public async Task<IEnumerable<Event>> GetEventsBySpeakerAsync(Guid speakerId)
    {
        return await this
            .Where(e => e.Sessions.Any(s => s.Speaker.Identity == speakerId))
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetEventsInPeriodAsync(DateTime start, DateTime end)
    {
        return await this
            .Where(e => e.Period.StartDate >= start && e.Period.EndDate <= end)
            .OrderBy(e => e.Period.StartDate)
            .ToListAsync();
    }

    public override void Update(Event item)
    {
        // Custom update logic for handling child entities
        CheckSessionEntries(item.Sessions);
        base.Update(item);
    }

    private void CheckSessionEntries(IEnumerable<Session> sessions)
    {
        foreach (var session in sessions)
        {
            var sessionEntry = DbContext.Entry(session);
            if (sessionEntry?.State == EntityState.Modified)
            {
                sessionEntry.State = EntityState.Added;
            }
        }
    }
}
~~~

### Entity Configuration

Define entity configurations using EF Core's fluent API:

~~~ c#
public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");
        builder.HasKey(x => x.Identity);
        builder.Property(x => x.Identity).HasColumnName("Id");
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(200);
        
        // Value object mapping
        builder.OwnsOne(x => x.Period, period =>
        {
            period.Property(p => p.StartDate).HasColumnName("StartDate");
            period.Property(p => p.EndDate).HasColumnName("EndDate");
        });
        
        // One-to-many relationship
        builder.HasMany(x => x.Sessions)
            .WithOne(x => x.Event)
            .HasForeignKey("FK_Event")
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.Navigation(x => x.Sessions).AutoInclude(true);
    }
}

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("Sessions");
        builder.HasKey(x => x.Identity);
        builder.Property(x => x.Identity).HasColumnName("Id");
        
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.Description)
            .HasMaxLength(500);
            
        builder.Property(x => x.Level);
        
        // Many-to-one relationships
        builder.HasOne(x => x.Event)
            .WithMany(x => x.Sessions)
            .HasForeignKey("FK_Event")
            .IsRequired();
            
        builder.HasOne(x => x.Speaker)
            .WithMany()
            .HasForeignKey("FK_Speaker")
            .IsRequired();
    }
}
~~~

## Testing with In-Memory Provider

~~~ c#
public class EventServiceTests
{
    private IPersistenceUnitFactory CreateTestFactory()
    {
        var factory = new PersistenceUnitFactory();
        
        var persistenceUnit = new EntityFrameworkInMemoryPersistenceUnit("TestUnit");
        persistenceUnit
            .AddClass<Event, Guid>()
            .AddClass<Speaker, Guid>();
        
        var attributes = new Dictionary<string, string>
        {
            { "databaseName", "TestDb" }
        };
        persistenceUnit.Initialize(attributes, new ConfigurationBuilder().Build());
        
        factory.AddPersistenceUnit(persistenceUnit);
        return factory;
    }
    
    [Fact]
    public async Task Should_Create_Event()
    {
        var factory = CreateTestFactory();
        using var dataContext = factory.OpenDataContext();
        var repository = new EventEFRepository(dataContext);
        
        var eventResult = Event.Create("Test Event", "Description", period);
        await repository.AddAsync(eventResult.Value);
        await dataContext.SaveChangesAsync();
        
        var loadedEvent = await repository.LoadAsync(eventResult.Value.Identity);
        Assert.NotNull(loadedEvent);
        Assert.Equal("Test Event", loadedEvent.Name);
    }
}
~~~

Go to the next step: [Summary](https://github.com/dogcane/ECO/blob/master/docs/Summary.md) of the docs.
