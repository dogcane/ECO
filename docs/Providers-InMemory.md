# In Memory Provider

The In Memory provider is a simple, lightweight persistence provider that stores data in memory using concurrent collections. It's perfect for testing, demos, and development scenarios where you don't need actual database persistence.

## Features

- **Simple Setup**: No database required, works out of the box
- **Thread-Safe**: Uses `ConcurrentDictionary` for thread-safe operations
- **Fast**: Direct memory access provides excellent performance
- **Testing Friendly**: Perfect for unit tests and integration tests
- **Transient Data**: Data is lost when the application stops

## Installation


Install the InMemory provider package:

```bash
dotnet add package ECO.Providers.EntityFramework
```

```xml
<PackageReference Include="ECO.Providers.InMemory" Version="3.0.0" />
```

## Configuration

### Fluent Configuration

Configure In-Memory provider using the fluent API in your startup:

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

### Repository Implementation

Create a typed repository by extending the base In Memory repository:

~~~ c#
public interface IEventRepository : IRepository<Event, Guid>
{
    // Add custom methods if needed
}

public class EventMemoryRepository : InMemoryRepository<Event, Guid>, IEventRepository
{
    public EventMemoryRepository(IDataContext dataContext)
        : base(dataContext)
    {
    }
    
    // Override methods if needed
    public override void Update(Event item)
    {
        // Custom update logic
        base.Update(item);
    }
}
~~~

## Usage Example

~~~ c#
public class EventService
{
    private readonly IDataContext _dataContext;
    private readonly IEventRepository _eventRepository;

    public EventService(IDataContext dataContext, IEventRepository eventRepository)
    {
        _dataContext = dataContext;
        _eventRepository = eventRepository;
    }

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

    public async Task<IEnumerable<Event>> GetAllEventsAsync()
    {
        return await _eventRepository.ToListAsync();
    }
}
~~~

## Data Storage

The In Memory provider uses static concurrent dictionaries to store data:

~~~ c#
protected static readonly ConcurrentDictionary<K, T> _EntitySet = new();
~~~

This means:
- Data is shared across all instances of the same repository type
- Data persists across different data context instances in the same application
- Data is lost when the application restarts

## Limitations

1. **No Persistence**: Data is lost on application restart
2. **Memory Usage**: All data is kept in memory
3. **No Relationships**: No foreign key constraints or relationship management
5. **No Concurrency Control**: Basic optimistic concurrency handling

## Best Use Cases

- **Unit Testing**: Fast, isolated tests without database setup
- **Integration Testing**: Quick tests of business logic
- **Prototyping**: Rapid development without database configuration
- **Development**: Local development without database dependencies
- **Demos**: Simple demonstrations and examples

## Performance Considerations

The In Memory provider is extremely fast for:
- Small to medium datasets
- Read-heavy workloads
- Testing scenarios

However, be aware of:
- Memory consumption with large datasets
- No data persistence across application restarts
- Limited scalability compared to database providers

Go to the next step: [NHibernate Provider](https://github.com/dogcane/ECO/blob/master/docs/Providers-NHibernate.md) or go to the [Summary](https://github.com/dogcane/ECO/blob/master/docs/Summary.md) of the docs.
