# NHibernate Provider

The NHibernate provider integrates ECO with NHibernate ORM, providing a mature and feature-rich persistence solution for .NET applications. It supports complex mapping scenarios, advanced querying capabilities, and robust transaction management.

## Features

- **Mature ORM**: Leverage NHibernate's proven object-relational mapping capabilities
- **Flexible Mapping**: Support for XML mappings, Fluent mappings, and mapping by code
- **Advanced Querying**: HQL, Criteria API, and LINQ support
- **Lazy Loading**: Efficient loading strategies for related entities
- **Caching**: First and second-level caching support
- **Transaction Management**: Robust transaction handling with proper isolation levels
- **Database Support**: Works with multiple database providers

## Installation

Install the NHibernate provider package:

```bash
dotnet add package ECO.Providers.NHibernate
```

```xml
<PackageReference Include="ECO.Providers.NHibernate" Version="2.0.0" />
```

## Configuration

## Configuration

### Fluent Configuration (Recommended)

Configure NHibernate provider using the fluent API in your startup:

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

### Repository Implementation

Create typed repositories extending the NHibernate base repository:

~~~ c#
public interface IEventRepository : IRepository<Event, Guid>
{
    Task<IEnumerable<Event>> GetEventsBySpeakerAsync(Speaker speaker);
    Task<int> GetEventsCountBySpeakerAsync(Speaker speaker);
}

public class EventNHRepository : NHRepository<Event, Guid>, IEventRepository
{
    public EventNHRepository(IDataContext dataContext) 
        : base(dataContext)
    {
    }

    public async Task<IEnumerable<Event>> GetEventsBySpeakerAsync(Speaker speaker)
    {
        return await GetCurrentSession()
            .QueryOver<Event>()
            .Where(ev => ev.Sessions.Any(s => s.Speaker == speaker))
            .ListAsync();
    }

    public async Task<int> GetEventsCountBySpeakerAsync(Speaker speaker)
    {
        return await GetCurrentSession()
            .QueryOver<Event>()
            .Where(ev => ev.Sessions.Any(s => s.Speaker == speaker))
            .RowCountAsync();
    }
}
~~~

## Database Support

NHibernate supports multiple database providers:
- SQL Server
- PostgreSQL
- MySQL
- Oracle
- SQLite
- And many others

Choose the appropriate dialect in your configuration.

Go to the next step: [EFCore Provider](https://github.com/dogcane/ECO/blob/master/docs/Providers-EF.md) or go to the [Summary](https://github.com/dogcane/ECO/blob/master/docs/Summary.md) of the docs.
