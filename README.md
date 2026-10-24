# ECO

<!---
[![NuGet](http://img.shields.io/nuget/vpre/ECO.svg?label=NuGet)](https://github.com/dogcane/ECO/)
--->

ECO is a comprehensive .NET library designed to simplify Domain-Driven Design (DDD) development by providing robust infrastructure components for persistence, repository patterns, and event sourcing. It eliminates the need to create boilerplate classes for entities, aggregate roots, and value objects, allowing developers to focus on business logic.

> **Requirements:** ECO 4.x targets **.NET 10 (`net10.0`)** and requires the .NET 10 SDK. Package versions are aligned with the .NET 10 wave: Microsoft.Extensions/EF Core 10.0.x, Npgsql 10.0.x, Marten 9.x and HotChocolate 16.x.

### Upgrading from ECO 3.x

- Retarget your projects to `net10.0`.
- Update Microsoft.Extensions, EF Core and ASP.NET Core packages to `10.0.x`, and Npgsql packages to `10.0.x`.
- The Marten provider now uses Marten 9.x. If you use the Newtonsoft.Json serializer, add the `Marten.Newtonsoft` package.
- The HotChocolate integration uses HotChocolate 16.x.

### Upgrading from ECO 4.0

- Repositories now expose async streaming through `ToAsyncEnumerable(cancellationToken)` (see [Async streaming](https://github.com/dogcane/ECO/blob/master/docs/Repositories.md#async-streaming)). The repository interfaces are unchanged, so existing custom repositories and mocks keep compiling.
- With .NET 10, `System.Linq.AsyncEnumerable` is part of the framework: remove the `System.Linq.Async` package to avoid duplicated extension methods.
- The NHibernate provider has no native async streaming, so its `ToAsyncEnumerable` materializes the result set before yielding.

## Key Features

- **🏗️ DDD Building Blocks**: Ready-to-use base classes for entities, aggregate roots, and value objects
- **🔄 Multiple Persistence Providers**: Support for Entity Framework Core, NHibernate, In-Memory, MongoDB, and Marten
- **📦 Repository Pattern**: Generic and typed repositories with full CRUD operations and LINQ support
- **🌊 Async Streaming**: Provider-native `IAsyncEnumerable<T>` streaming through `ToAsyncEnumerable(cancellationToken)`
- **⚡ Event Sourcing**: Built-in event sourcing capabilities with versioning and event tracking
- **🏢 Persistence Units**: Flexible architecture for managing multiple data contexts
- **🔧 Dependency Injection**: Easy integration with .NET's built-in DI container
- **🧪 Testing Support**: In-memory providers for unit and integration testing
- **📊 Transaction Management**: Robust transaction handling across multiple contexts

## Quick Start

### 1. Install ECO

```bash
# Core library and Microsoft DI integration
dotnet add package ECO.Core
dotnet add package ECO.Core.Integrations.Microsoft.DependencyInjection

# Entity Framework Core provider plus a database provider
dotnet add package ECO.Core.Providers.EntityFramework
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

# OR In-Memory
dotnet add package ECO.Core.Providers.InMemory

# OR NHibernate
dotnet add package ECO.Core.Providers.NHibernate

# OR Marten
dotnet add package ECO.Core.Providers.Marten

# OR MongoDB
dotnet add package ECO.Providers.MongoDB
```

### 2. Define Your Domain Model

```c#
public class Event : AggregateRoot<Guid>
{
    public virtual string Name { get; protected set; }
    public virtual string Description { get; protected set; }
    public virtual Period Period { get; protected set; }
    public virtual IEnumerable<Session> Sessions => _sessions;

    public static OperationResult<Event> Create(string name, string description, Period period)
    {
        // Validation logic
        return new Event(name, description, period);
    }

    public virtual OperationResult<Session> AddSession(string title, Speaker speaker)
    {
        var session = Session.Create(this, title, speaker);
        if (session.Success)
            _sessions.Add(session.Value);
        return session;
    }
}
```

### 3. Create Repository Interface

```c#
public interface IEventRepository : IRepository<Event, Guid>
{
    Task<IEnumerable<Event>> GetEventsByPeriodAsync(DateTime start, DateTime end);
}
```

### 4. Configure Services

```c#
using ECO.Integrations.Microsoft.DependencyInjection;
using ECO.Providers.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;

public void ConfigureServices(IServiceCollection services)
{
    services.AddDataContext(options =>
    {
        options.UseEntityFramework<ECODbContext>("MainUnit", opt =>
        {
            opt.DbContextOptions.UseSqlServer(
                Configuration.GetConnectionString("sqlserver"));
        });
    });

    // Register repositories. AddDataContext also registers IDataContext as scoped.
    services.AddScoped<IEventRepository, EventEFRepository>();
}
```

### 5. Use in Your Application

```c#
public class EventService
{
    private readonly IDataContext _dataContext;
    private readonly IEventRepository _eventRepository;

    public async Task<Event> CreateEventAsync(string name, string description, Period period)
    {
        using var transaction = await _dataContext.BeginTransactionAsync();
        
        var eventResult = Event.Create(name, description, period);
        if (!eventResult.Success)
            throw new ValidationException(eventResult.Messages);

        await _eventRepository.AddAsync(eventResult.Value);
        await _dataContext.SaveChangesAsync();
        
        transaction.Commit();
        return eventResult.Value;
    }
}
```

## Async Streaming

Repositories can stream aggregates without loading the whole result set:

```c#
public class EventStreamer
{
    private readonly IEventRepository _eventRepository;

    public async IAsyncEnumerable<Event> StreamEventsAsync(
        DateTime from,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var evt in _eventRepository
            .Where(e => e.Period.StartDate >= from)
            .AsAsyncEnumerable()
            .WithCancellation(cancellationToken))
        {
            yield return evt;
        }
    }
}
```

`_eventRepository.ToAsyncEnumerable(cancellationToken)` streams the whole aggregate set with the provider-native implementation (EF Core, Marten, MongoDB), with a synchronous fallback for custom providers and NHibernate materialization. See [Async streaming](https://github.com/dogcane/ECO/blob/master/docs/Repositories.md#async-streaming) for the API, per-provider behavior and lifetime warnings: the persistence context must stay alive while the stream is enumerated.

## Event Sourcing Support

ECO includes comprehensive event sourcing capabilities:

```c#
public class Order : ESAggregateRoot<string>
{
    public static Order CreateNew(string id, string customerName)
    {
        var order = new Order();
        var @event = new OrderCreated(id, customerName);
        order.OnApply(@event, order.Apply);
        return order;
    }

    public void AddItem(string productId, int quantity, decimal price)
    {
        var @event = new ItemAdded(Identity, productId, quantity, price);
        OnApply(@event, Apply);
    }

    private void Apply(OrderCreated @event) => /* Update state */;
    private void Apply(ItemAdded @event) => /* Update state */;
}
```

## Available Providers

| Provider | Package | Description |
|----------|---------|-------------|
| **Entity Framework Core** | `ECO.Core.Providers.EntityFramework` | Generic EF Core provider; pair it with `Microsoft.EntityFrameworkCore.SqlServer` or `Npgsql.EntityFrameworkCore.PostgreSQL` |
| **In-Memory** | `ECO.Core.Providers.InMemory` | Fast in-memory storage for testing |
| **NHibernate** | `ECO.Core.Providers.NHibernate` | Mature ORM with advanced features |
| **MongoDB** | `ECO.Providers.MongoDB` | Document database support |
| **Marten** | `ECO.Core.Providers.Marten` | Marten 9.x / PostgreSQL document DB and event store |

The core abstractions ship in `ECO.Core`, DI integration in `ECO.Core.Integrations.Microsoft.DependencyInjection`, and event-sourcing abstractions in `ECO.Core.EventSourcing`.

## Architecture Benefits

- **🎯 Focus on Business Logic**: Infrastructure concerns are handled by ECO
- **🔀 Provider Flexibility**: Switch between providers without changing domain code
- **📈 Scalable Design**: Support for multiple persistence units and contexts
- **🧪 Testable**: Built-in support for testing with in-memory providers
- **📐 SOLID Principles**: Clean separation of concerns and dependency inversion
- **🌐 Cross-Platform**: Runs on Windows, Linux, and macOS with .NET 10

## Documentation

Comprehensive documentation is available:

- **[Getting Started Guide](https://github.com/dogcane/ECO/blob/master/docs/Getting-started.md)** - Step-by-step tutorial
- **[Aggregates & Entities](https://github.com/dogcane/ECO/blob/master/docs/Aggregate-Entities.md)** - Domain modeling patterns
- **[Repositories](https://github.com/dogcane/ECO/blob/master/docs/Repositories.md)** - Data access patterns
- **[Persistence Units](https://github.com/dogcane/ECO/blob/master/docs/Units-Contexts.md)** - Context management
- **[Event Sourcing](https://github.com/dogcane/ECO/blob/master/docs/Event-Sourcing.md)** - Event sourcing implementation
- **[Providers](https://github.com/dogcane/ECO/blob/master/docs/Providers.md)** - Persistence provider guides
- **[Complete Documentation](https://github.com/dogcane/ECO/blob/master/docs/Summary.md)** - All documentation links

## Contributing

We welcome contributions! Please see our contributing guidelines and code of conduct.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
