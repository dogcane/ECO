# Event Sourcing

ECO provides built-in support for Event Sourcing through the `ECO.EventSourcing` namespace. Event Sourcing is a pattern where state changes are captured as a sequence of events, allowing you to rebuild the current state by replaying these events.

## Key Concepts

### ESAggregateRoot

The `ESAggregateRoot<T>` is the base class for aggregates that support event sourcing:

~~~ c#
public abstract class ESAggregateRoot<T> : AggregateRoot<T>, IESAggregateRoot<T>
{
    public long Version { get; protected set; }
    
    protected void OnApply<E>(E @event, Action<E> applyHandler);
    
    IEnumerable<object> GetUncommittedEvents();
    void ClearUncommittedEvents();
}
~~~

### IESAggregateRoot Interface

Defines the contract for event sourced aggregates:

~~~ c#
public interface IESAggregateRoot<T> : IAggregateRoot<T>
{
    long Version { get; }
    IEnumerable<object> GetUncommittedEvents();
    void ClearUncommittedEvents();
}
~~~

### IESRepository Interface

Specialized repository interface for event sourced aggregates:

~~~ c#
public interface IESRepository<T, K> : IPersistenceManager<T, K>
    where T : class, IESAggregateRoot<K>
    where K : notnull, IEquatable<K>
{
    T? Load(K identity);
    ValueTask<T?> LoadAsync(K identity);
    
    void Save(T item);
    Task SaveAsync(T item);
    
    IEnumerable<dynamic> LoadEvents(K identity);
    Task<IEnumerable<dynamic>> LoadEventsAsync(K identity);
}
~~~

## Creating Event Sourced Aggregates

### Define Your Events

First, define your domain events as simple classes:

~~~ c#
public record OrderCreated(string Id, string Name, string Surname, string Address);
public record OrderDetailAdded(string OrderId, int Sku, string Description, int Quantity, float Amount);
public record OrderDetailRemoved(string OrderId, int Sku, int Quantity);
public record OrderPrepared(string OrderId);
public record OrderShipped(string OrderId);
~~~

### Implement the Aggregate

Create your aggregate by inheriting from `ESAggregateRoot<T>`:

~~~ c#
public class Order : ESAggregateRoot<string>
{
    private readonly List<OrderDetail> _items = new();

    public string? Name { get; private set; }
    public string? Surname { get; private set; }
    public string? Address { get; private set; }
    public IEnumerable<OrderDetail> Items => _items;
    public OrderStatus Status { get; private set; }
    public float Total => Items.Sum(item => item.TotalAmount);

    // Private constructor for rehydration
    private Order() : base() { }

    // Factory method for creating new orders
    public static Order? CreateNew(string id, string name, string surname, string address)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || 
            string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(address))
            return null;

        var order = new Order();
        var @event = new OrderCreated(id, name, surname, address);
        order.OnApply(@event, order.Apply);
        return order;
    }

    // Business methods that generate events
    public bool AddDetail(int sku, string description, int quantity, float amount)
    {
        if (sku <= 0 || string.IsNullOrEmpty(description) || quantity <= 0)
            return false;

        if (Status != OrderStatus.New)
            return false;

        var @event = new OrderDetailAdded(Identity!, sku, description, quantity, amount);
        OnApply(@event, Apply);
        return true;
    }

    public bool Prepare()
    {
        if (Status != OrderStatus.New)
            return false;

        var @event = new OrderPrepared(Identity!);
        OnApply(@event, Apply);
        return true;
    }

    // Event handlers (private methods that apply state changes)
    private void Apply(OrderCreated @event)
    {
        Identity = @event.Id;
        Name = @event.Name;
        Surname = @event.Surname;
        Address = @event.Address;
        Status = OrderStatus.New;
    }

    private void Apply(OrderDetailAdded @event)
    {
        var existingItem = _items.FirstOrDefault(it => it.Sku == @event.Sku);
        if (existingItem != null)
        {
            existingItem.Quantity += @event.Quantity;
        }
        else
        {
            var detail = new OrderDetail
            {
                Sku = @event.Sku,
                Amount = @event.Amount,
                Description = @event.Description,
                Quantity = @event.Quantity
            };
            _items.Add(detail);
        }
    }

    private void Apply(OrderPrepared @event)
    {
        Status = OrderStatus.Prepared;
    }
}
~~~

## Repository Implementation

### Define Repository Interface

~~~ c#
public interface IOrderRepository : IESRepository<Order, string>
{
    // Add custom methods if needed
}
~~~

### Implementation with Marten Provider

~~~ c#
public class OrderMartenRepository : MartenESRepository<Order, string>, IOrderRepository
{
    public OrderMartenRepository(IDataContext dataContext) 
        : base(dataContext)
    {
    }
}
~~~

## Usage Examples

### Creating and Saving Aggregates

~~~ c#
public class OrderService
{
    private readonly IDataContext _dataContext;
    private readonly IOrderRepository _orderRepository;

    public OrderService(IDataContext dataContext, IOrderRepository orderRepository)
    {
        _dataContext = dataContext;
        _orderRepository = orderRepository;
    }

    public async Task<string> CreateOrderAsync(string id, string name, string surname, string address)
    {
        using var transaction = await _dataContext.BeginTransactionAsync();
        
        var order = Order.CreateNew(id, name, surname, address);
        if (order == null)
            throw new ArgumentException("Invalid order parameters");

        await _orderRepository.SaveAsync(order);
        
        transaction.Commit();
        return order.Identity!;
    }

    public async Task AddItemAsync(string orderId, int sku, string description, int quantity, float amount)
    {
        using var transaction = await _dataContext.BeginTransactionAsync();
        
        var order = await _orderRepository.LoadAsync(orderId);
        if (order == null)
            throw new InvalidOperationException("Order not found");

        if (!order.AddDetail(sku, description, quantity, amount))
            throw new InvalidOperationException("Cannot add item to order");

        await _orderRepository.SaveAsync(order);
        
        transaction.Commit();
    }
}
~~~

### Loading Aggregates

~~~ c#
public async Task<Order?> GetOrderAsync(string orderId)
{
    return await _orderRepository.LoadAsync(orderId);
}
~~~

### Loading Event History

~~~ c#
public async Task<IEnumerable<dynamic>> GetOrderHistoryAsync(string orderId)
{
    return await _orderRepository.LoadEventsAsync(orderId);
}
~~~

## Key Patterns

### OnApply Method

The `OnApply` method is the core of event sourcing in ECO:

1. **Applies the event** to change the aggregate state
2. **Tracks the event** as uncommitted
3. **Increments the version** for optimistic concurrency

~~~ c#
protected void OnApply<E>(E @event, Action<E> applyHandler)
{
    ArgumentNullException.ThrowIfNull(@event);
    ArgumentNullException.ThrowIfNull(applyHandler);
    applyHandler(@event);
    _uncommittedEvents.Add(@event);
    Version++;
}
~~~

### Event Handler Pattern

Event handlers should be private methods that only modify state:

~~~ c#
private void Apply(OrderCreated @event)
{
    // Only modify state, no business logic
    Identity = @event.Id;
    Name = @event.Name;
    Status = OrderStatus.New;
}
~~~

### Uncommitted Events

Events are tracked until they're persisted:

~~~ c#
// After business operations
var events = aggregate.GetUncommittedEvents();
// Events: [OrderCreated, OrderDetailAdded, OrderDetailAdded]

// After saving
await repository.SaveAsync(aggregate);
// aggregate.GetUncommittedEvents() returns empty collection
~~~

## Provider Support

### Marten Provider

ECO includes a Marten provider for PostgreSQL event sourcing:

~~~ c#
var persistenceUnit = new MartenPersistenceUnit("EventStoreUnit", loggerFactory);
persistenceUnit.AddClass<Order, string>();

// Configure Marten document store
var documentStore = DocumentStore.For(options =>
{
    options.Connection(connectionString);
    options.Events.StreamIdentity = StreamIdentity.AsString;
    options.Projections.Snapshot<Order>(SnapshotLifecycle.Inline);
});
~~~

### Custom Provider Implementation

You can implement custom event sourcing providers by:

1. Implementing `IESRepository<T, K>`
2. Storing events in your preferred event store
3. Implementing event replay for aggregate rehydration

## Best Practices

1. **Keep Events Immutable**: Use records or immutable classes for events
2. **Design Events for the Future**: Consider versioning and schema evolution
3. **Separate Command and Query**: Use CQRS pattern with event sourcing
4. **Handle Concurrency**: Use version numbers for optimistic concurrency control
5. **Event Handlers Pure**: Keep apply methods side-effect free
6. **Snapshot Large Aggregates**: Consider snapshots for performance with many events

## Testing Event Sourced Aggregates

~~~ c#
[Fact]
public void Should_Create_Order_And_Track_Events()
{
    // Arrange & Act
    var order = Order.CreateNew("123", "John", "Doe", "123 Main St");

    // Assert
    Assert.NotNull(order);
    Assert.Equal("123", order.Identity);
    Assert.Equal("John", order.Name);
    Assert.Equal(1, order.Version);
    
    var events = ((IESAggregateRoot<string>)order).GetUncommittedEvents();
    Assert.Single(events);
    Assert.IsType<OrderCreated>(events.First());
}

[Fact]
public void Should_Add_Detail_And_Increment_Version()
{
    // Arrange
    var order = Order.CreateNew("123", "John", "Doe", "123 Main St");
    
    // Act
    var result = order.AddDetail(1001, "Widget", 2, 10.00f);
    
    // Assert
    Assert.True(result);
    Assert.Equal(2, order.Version);
    Assert.Single(order.Items);
    
    var events = ((IESAggregateRoot<string>)order).GetUncommittedEvents();
    Assert.Equal(2, events.Count());
}
~~~

Event sourcing with ECO provides a powerful foundation for building systems that require full audit trails, temporal queries, and complex business event handling.

Go to the next step: [Summary](https://github.com/dogcane/ECO/blob/master/docs/Summary.md) of the docs.
