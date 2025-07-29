# ECO

<!---
[![NuGet](http://img.shields.io/nuget/vpre/ECO.svg?label=NuGet)](https://github.com/dogcane/ECO/)
--->

ECO is a comprehensive .NET library designed to simplify Domain-Driven Design (DDD) development by providing robust infrastructure components for persistence, repository patterns, and event sourcing. It eliminates the need to create boilerplate classes for entities, aggregate roots, and value objects, allowing developers to focus on business logic.

## Key Features

- **🏗️ DDD Building Blocks**: Ready-to-use base classes for entities, aggregate roots, and value objects
- **🔄 Multiple Persistence Providers**: Support for Entity Framework Core, NHibernate, In-Memory, MongoDB, and Marten
- **📦 Repository Pattern**: Generic and typed repositories with full CRUD operations and LINQ support
- **⚡ Event Sourcing**: Built-in event sourcing capabilities with versioning and event tracking
- **🏢 Persistence Units**: Flexible architecture for managing multiple data contexts
- **🔧 Dependency Injection**: Easy integration with .NET's built-in DI container
- **🧪 Testing Support**: In-memory providers for unit and integration testing
- **📊 Transaction Management**: Robust transaction handling across multiple contexts

## Quick Start

### 1. Install ECO

```bash
# Core library
dotnet add package ECO

# Choose your provider
dotnet add package ECO.Providers.EntityFramework.SqlServer
# OR
dotnet add package ECO.Providers.InMemory
# OR
dotnet add package ECO.Providers.NHibernate
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
public void ConfigureServices(IServiceCollection services)
{
    // Register persistence unit factory
    services.AddSingleton<IPersistenceUnitFactory>(serviceProvider =>
    {
        var factory = new PersistenceUnitFactory(loggerFactory);
        
        var persistenceUnit = new EntityFrameworkSqlServerPersistenceUnit("MainUnit", loggerFactory);
        persistenceUnit.AddClass<Event, Guid>();
        
        var attributes = new Dictionary<string, string>
        {
            { "connectionString", connectionString }
        };
        persistenceUnit.Initialize(attributes, Configuration);
        
        factory.AddPersistenceUnit(persistenceUnit);
        return factory;
    });

    // Register repositories and data context
    services.AddScoped<IEventRepository, EventEFRepository>();
    services.AddScoped<IDataContext>(provider => 
        provider.GetRequiredService<IPersistenceUnitFactory>().OpenDataContext());
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
| **Entity Framework Core** | `ECO.Providers.EntityFramework.SqlServer` | Production-ready EF Core with SQL Server |
| **In-Memory** | `ECO.Providers.InMemory` | Fast in-memory storage for testing |
| **NHibernate** | `ECO.Providers.NHibernate` | Mature ORM with advanced features |
| **MongoDB** | `ECO.Providers.MongoDB` | Document database support |
| **Marten** | `ECO.Providers.Marten` | PostgreSQL document DB + Event Store |

## Architecture Benefits

- **🎯 Focus on Business Logic**: Infrastructure concerns are handled by ECO
- **🔀 Provider Flexibility**: Switch between providers without changing domain code
- **📈 Scalable Design**: Support for multiple persistence units and contexts
- **🧪 Testable**: Built-in support for testing with in-memory providers
- **📐 SOLID Principles**: Clean separation of concerns and dependency inversion
- **🌐 Cross-Platform**: Works with .NET Core, .NET 5+, and .NET Framework

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
