# Marten Provider

The Marten provider integrates ECO with Marten, a .NET document database and event store using PostgreSQL as the storage engine. It supports both traditional document storage and event sourcing patterns.

## Features

- **Document Database**: PostgreSQL-backed document storage with JSONB
- **Event Sourcing**: Built-in event sourcing capabilities
- **LINQ Support**: Rich querying with LINQ to PostgreSQL
- **Schema Management**: Automatic schema generation and migration
- **Performance**: Optimized for high-performance scenarios
- **Multi-tenancy**: Built-in multi-tenant support

## Installation

Add the Marten provider package to your project:

```bash
dotnet add package ECO.Providers.Marten
```

```xml
<PackageReference Include="ECO.Providers.Marten" Version="3.0.0" />
```

NB: Also ensure you have PostgreSQL connectivity:

## Configuration

### Fluent Configuration

Configure Marten using the fluent API:

~~~ c#
using ECO.Providers.Marten.Configuration;

builder.Services.AddDataContext(options =>
{
    options.UseMarten("ecosampleapp.marten", opt =>
    {
        // Configure Marten store options
        opt.StoreOptions.Connection("Host=localhost;Database=ECOSampleApp;Username=eco;Password=password");
        
        // Auto-discover aggregate types from assembly
        opt.AddAssemblyFromType<Event>();
        
        // Or add specific aggregate types
        opt.AddClass<Event, Guid>();
        opt.AddClass<Speaker, Guid>();
        opt.AddClass<Session, Guid>();
        
        // Configure document options
        opt.StoreOptions.Schema.For<Event>().Index(x => x.StartDate);
        opt.StoreOptions.Schema.For<Speaker>().Index(x => x.Email);
        
        // Optional: Add persistence unit listeners
        opt.AddListener<MyCustomListener>();
    });
});
~~~

### Repository Implementation

#### Document Repository

Create repositories for document storage:

~~~ c#
using ECO.Data;
using ECO.Providers.Marten;

public class EventMartenRepository : MartenRepository<Event, Guid>
{
    public EventMartenRepository(IDataContext dataContext) : base(dataContext)
    {
    }

    public async Task<IEnumerable<Event>> FindByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await DocumentSession.Query<Event>()
            .Where(e => e.StartDate >= startDate && e.EndDate <= endDate)
            .ToListAsync();
    }

    public async Task<Event?> FindBySlugAsync(string slug)
    {
        return await DocumentSession.Query<Event>()
            .FirstOrDefaultAsync(e => e.Slug == slug);
    }
}
~~~

#### Event Sourcing Repository

For event sourcing scenarios, use the ES repository:

~~~ c#
using ECO.Data;
using ECO.Providers.Marten;

public class OrderMartenRepository : MartenESRepository<Order, string>
{
    public OrderMartenRepository(IDataContext dataContext) : base(dataContext)
    {
    }

    // Additional domain-specific methods can be added here
    public async Task<IEnumerable<Order>> FindOrdersByCustomerAsync(string customerId)
    {
        // Note: This would load full aggregates from events
        // Consider using projections for queries
        var orderIds = await DocumentSession.Query<OrderProjection>()
            .Where(p => p.CustomerId == customerId)
            .Select(p => p.Id)
            .ToListAsync();

        var orders = new List<Order>();
        foreach (var orderId in orderIds)
        {
            var order = await LoadAsync(orderId);
            if (order != null)
                orders.Add(order);
        }
        return orders;
    }
}
~~~

## Document Storage

### Document Structure

Aggregates are stored as JSONB documents in PostgreSQL:

~~~ sql
-- Events are stored in mt_doc_event table
SELECT id, data FROM mt_doc_event WHERE id = 'some-guid';

-- Result:
{
  "Id": "550e8400-e29b-41d4-a716-446655440000",
  "Title": "Azure Conference 2024",
  "Description": "Annual Azure conference",
  "StartDate": "2024-03-15T09:00:00Z",
  "EndDate": "2024-03-17T17:00:00Z",
  "Status": "Published"
}
~~~

### Schema Customization

Customize document schema and indexing:

~~~ c#
storeOptions.Schema.For<Event>()
    .Index(x => x.StartDate)
    .Index(x => x.Location)
    .GinIndexJsonData() // For full-text search
    .DocumentAlias("events");

storeOptions.Schema.For<Speaker>()
    .Index(x => x.Email, x => x.IsUnique = true)
    .Index(x => x.LastName);
~~~

## Event Sourcing

### Event Sourced Aggregate

Define aggregates that inherit from ESAggregateRoot:

~~~ c#
public class Order : ESAggregateRoot<string>
{
    public string CustomerId { get; private set; }
    public decimal Total { get; private set; }
    public OrderStatus Status { get; private set; }
    
    private readonly List<OrderLine> _lines = new();
    public IReadOnlyList<OrderLine> Lines => _lines.AsReadOnly();

    public Order(string id, string customerId) : base(id)
    {
        RaiseEvent(new OrderCreated(id, customerId));
    }

    public void AddLine(string productId, int quantity, decimal price)
    {
        RaiseEvent(new OrderLineAdded(Identity, productId, quantity, price));
    }

    // Event handlers
    protected override void OnEventApplied(object @event)
    {
        switch (@event)
        {
            case OrderCreated e:
                CustomerId = e.CustomerId;
                Status = OrderStatus.Created;
                break;
            case OrderLineAdded e:
                _lines.Add(new OrderLine(e.ProductId, e.Quantity, e.Price));
                RecalculateTotal();
                break;
        }
    }
}
~~~

### Working with Events

Load and save event sourced aggregates:

~~~ c#
public class OrderService
{
    private readonly IESRepository<Order, string> _orderRepository;

    public OrderService(IESRepository<Order, string> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order> CreateOrderAsync(string customerId)
    {
        var orderId = Guid.NewGuid().ToString();
        var order = new Order(orderId, customerId);
        
        await _orderRepository.SaveAsync(order);
        return order;
    }

    public async Task AddLineToOrderAsync(string orderId, string productId, int quantity, decimal price)
    {
        var order = await _orderRepository.LoadAsync(orderId);
        if (order == null) throw new OrderNotFoundException();
        
        order.AddLine(productId, quantity, price);
        await _orderRepository.SaveAsync(order);
    }
}
~~~

## Common Patterns

### Repository with Projections

~~~ c#
public interface IEventRepository : IRepository<Event, Guid>
{
    Task<IEnumerable<EventSummary>> GetEventSummariesAsync();
    Task<Event?> FindBySlugAsync(string slug);
}

public class EventMartenRepository : MartenRepository<Event, Guid>, IEventRepository
{
    public EventMartenRepository(IDataContext dataContext) : base(dataContext) { }

    public async Task<IEnumerable<EventSummary>> GetEventSummariesAsync()
    {
        // Use projection for efficient queries
        return await DocumentSession.Query<EventSummary>().ToListAsync();
    }

    public async Task<Event?> FindBySlugAsync(string slug)
    {
        return await DocumentSession.Query<Event>()
            .FirstOrDefaultAsync(e => e.Slug == slug);
    }
}
~~~

## See Also

- [Event Sourcing](Event-Sourcing.md) - Event sourcing concepts and patterns
- [Providers Overview](Providers.md) - Overview of all available providers
- [Getting Started](Getting-started.md) - Basic ECO concepts and setup
- [Repositories](Repositories.md) - Repository pattern implementation
- [Providers-EF](Providers-EF.md) - Entity Framework Core provider
