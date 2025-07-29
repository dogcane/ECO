# ECO Providers

ECO is built with a "provider model" in mind. It's possible to define your own persistence context and configure the desired provider.
ECO supports multiple persistence providers to accommodate different storage requirements:

- [In Memory](https://github.com/dogcane/ECO/blob/master/docs/Providers-InMemory.md) - Lightweight in-memory storage for testing and development
- [Entity Framework Core](https://github.com/dogcane/ECO/blob/master/docs/Providers-EF.md) - Relational database support via EF Core
- [NHibernate](https://github.com/dogcane/ECO/blob/master/docs/Providers-NHibernate.md) - Advanced ORM features with NHibernate
- [MongoDB](https://github.com/dogcane/ECO/blob/master/docs/Providers-MongoDB.md) - Document-based storage with MongoDB
- [Marten](https://github.com/dogcane/ECO/blob/master/docs/Providers-Marten.md) - PostgreSQL document database and event store

## Microsoft DI Container Configuration Examples

ECO integrates seamlessly with Microsoft's Dependency Injection container. Below are examples showing how to configure each provider using the `ServiceCollection` in your `Program.cs` or `Startup.cs`:

### In-Memory Provider

Perfect for testing and development scenarios:

~~~ c#
using ECO.Providers.InMemory.Configuration;

builder.Services.AddDataContext(options =>
{
    options.UseInMemory("ecosampleapp.inmemory", opt => 
        opt.AddAssemblyFromType<YourDomain.AssemblyMarker>());
});

// Register repositories
builder.Services.AddScoped<IEventRepository, EventMemoryRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerMemoryRepository>();
~~~

### Entity Framework Core Provider

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

### NHibernate Provider

#### SQL Server
~~~ c#
using ECO.Providers.NHibernate.Configuration;
using ECO.Providers.NHibernate.Utils;

builder.Services.AddDataContext(options =>
{
    options.UseNHibernate("ecosampleapp.nh.sqlserver", opt =>
    {
        opt.Configuration.DataBaseIntegration(config =>
        {
            config.ConnectionString = builder.Configuration.GetConnectionString("sqlserver");
            config.Dialect<NHibernate.Dialect.MsSql2012Dialect>();
            config.Driver<NHibernate.Driver.SqlClientDriver>();
        }).AddAssemblyExtended(typeof(YourInfrastructure.AssemblyMarker).Assembly);
    });
});

// Register repositories
builder.Services.AddScoped<IEventRepository, EventNHRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerNHRepository>();
~~~

#### PostgreSQL
~~~ c#
using ECO.Providers.NHibernate.Configuration;
using ECO.Providers.NHibernate.Utils;

builder.Services.AddDataContext(options =>
{
    options.UseNHibernate("ecosampleapp.nh.postgresql", opt =>
    {
        opt.Configuration.DataBaseIntegration(config =>
        {
            config.ConnectionString = builder.Configuration.GetConnectionString("postgres");
            config.Dialect<NHibernate.Dialect.PostgreSQL83Dialect>();
            config.Driver<NHibernate.Driver.NpgsqlDriver>();
        })
        .SetNamingStrategy(new YourCustomNamingStrategy())
        .AddAssemblyExtended(typeof(YourInfrastructure.AssemblyMarker).Assembly)
        .SetProperty("default_schema", "public");
    });
});

// Register repositories
builder.Services.AddScoped<IEventRepository, EventNHRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerNHRepository>();
~~~

### MongoDB Provider

~~~ c#
using ECO.Providers.MongoDB.Configuration;

// Configure GUID serialization for MongoDB
MongoDB.Bson.Serialization.BsonSerializer.RegisterSerializer(
    new MongoDB.Bson.Serialization.Serializers.GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard)
);

builder.Services.AddDataContext(options =>
{
    options.UseMongoDB("ecosampleapp.mongodb", opt =>
    {
        opt.ConnectionString = builder.Configuration.GetConnectionString("mongo");
        opt.DatabaseName = "YourDatabaseName";
        opt.AddAssemblyFromType<YourDomain.AssemblyMarker>();
        opt.MappingAssemblies = "YourInfrastructure.DAL.MongoDB";
    });
});

// Register repositories
builder.Services.AddScoped<IEventRepository, EventMongoRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerMongoRepository>();
~~~

### Marten Provider

~~~ c#
using ECO.Providers.Marten.Configuration;
using ECO.Providers.Marten.Utils;
using JasperFx.Core;
using Marten.Services.Json;
using System.Text.Json.Serialization;

builder.Services.AddDataContext(options =>
{
    options.UseMarten("ecosampleapp.marten", opt =>
    {
        opt.AddAssemblyFromType<YourDomain.AssemblyMarker>();
        opt.StoreOptions.Connection(builder.Configuration.GetConnectionString("marten"));        
        opt.StoreOptions.AutoCreateSchemaObjects = JasperFx.AutoCreate.CreateOrUpdate;
        
        // Configure JSON serializer
        var serializer = new Marten.Services.SystemTextJsonSerializer
        {
            EnumStorage = EnumStorage.AsString
        };
        serializer.Configure(_ =>
        {
            _.TypeInfoResolver = new NonPublicContractResolver();
            _.ReferenceHandler = ReferenceHandler.IgnoreCycles;            
        });
        opt.StoreOptions.Serializer(serializer);
    });
});

// Register repositories
builder.Services.AddScoped<IEventRepository, EventMartenRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerMartenRepository>();
~~~

### Configuration File Approach

You can also use configuration files instead of fluent configuration:

~~~ c#
builder.Services.AddDataContext(options =>
{
    options.UseConfiguration(builder.Configuration);
});
~~~

This approach reads provider configuration from `appsettings.json` or other configuration sources.

### Generic Repository Registration (Work in Progress)
~~~ c#
// Future feature - automatic repository registration
// builder.Services.AddRepository<Event, Guid>();
// builder.Services.AddRepository<Speaker, Guid>();
~~~

## Choosing the Right Provider

- **In-Memory**: Testing, development, and temporary storage
- **Entity Framework Core**: Relational databases with strong typing and migrations
- **NHibernate**: Advanced ORM features, complex mappings, and enterprise scenarios
- **MongoDB**: Document-based storage, flexible schemas, and horizontal scaling
- **Marten**: PostgreSQL-based document storage with event sourcing capabilities

Go to the next step: [In Memory Provider](https://github.com/dogcane/ECO/blob/master/docs/Providers-InMemory.md) or go to the [Summary](https://github.com/dogcane/ECO/blob/master/docs/Summary.md) of the docs.
