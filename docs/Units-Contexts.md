# Persistence units and contexts

A persistence unit in ECO represents a storage backend (e.g., a relational database) and defines all the classes (aggregate roots) that are persisted within it. It's the central configuration point that ties together your domain model with a specific persistence provider.

## What is a Persistence Unit?

A persistence unit acts as a container that:

- Defines which aggregate root classes are managed
- Configures the persistence provider (InMemory, Entity Framework, NHibernate, etc.)
- Manages the creation of persistence contexts
- Handles lifecycle events through listeners

## Key Interfaces

### IPersistenceUnit

The core interface that defines a persistence unit:

~~~ c#
public interface IPersistenceUnit
{
    string Name { get; }
    IEnumerable<Type> Classes { get; }
    
    void Initialize(IDictionary<string, string> extendedAttributes, IConfiguration configuration);
    IPersistenceContext CreateContext();
    
    IPersistenceUnit AddClass<T, K>() where T : class, IAggregateRoot<K>;
    IPersistenceUnit RemoveClass<T, K>() where T : class, IAggregateRoot<K>;
    
    IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext);
    IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext);
}
~~~

### IPersistenceContext

Represents a session with the persistence store:

~~~ c#
public interface IPersistenceContext : IDisposable
{
    Guid PersistenceContextId { get; }
    IPersistenceUnit PersistenceUnit { get; }
    IDataTransaction? Transaction { get; }
    
    void Attach<T>(IAggregateRoot<T> entity);
    void Detach<T>(IAggregateRoot<T> entity);
    void Refresh<T>(IAggregateRoot<T> entity);
    
    PersistenceState GetPersistenceState<T>(IAggregateRoot<T> entity);
    IDataTransaction BeginTransaction();
    Task<IDataTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    void SaveChanges();
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
~~~

## Data Context

The `IDataContext` serves as the main entry point for working with persistence units. It manages multiple persistence contexts and coordinates transactions across them.

~~~ c#
public interface IDataContext : IDisposable
{
    Guid DataContextId { get; }
    ITransactionContext? Transaction { get; }
    
    void SaveChanges();
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    
    ITransactionContext BeginTransaction(bool autoCommit = false);
    Task<ITransactionContext> BeginTransactionAsync(bool autoCommit = false, CancellationToken cancellationToken = default);
    
    IPersistenceContext GetCurrentContext<T>();
    IPersistenceContext GetCurrentContext(Type entityType);
    
    void Attach<T>(IAggregateRoot<T> entity);
    void Detach<T>(IAggregateRoot<T> entity);
    PersistenceState GetPersistenceState<T>(IAggregateRoot<T> entity);
}
~~~

## Persistence Unit Factory

The `IPersistenceUnitFactory` manages all registered persistence units and creates data contexts:

~~~ c#
public interface IPersistenceUnitFactory
{
    IPersistenceUnitFactory AddPersistenceUnit(IPersistenceUnit persistenceUnit);
    IPersistenceUnit GetPersistenceUnit(string name);
    IPersistenceUnit GetPersistenceUnit<T>();
    IPersistenceUnit GetPersistenceUnit(Type entityType);
    IDataContext OpenDataContext();
}
~~~

## Usage Example

Here's how to set up and use persistence units:

~~~ c#
// Create persistence unit factory
var factory = new PersistenceUnitFactory(loggerFactory);

// Create and configure a persistence unit
var persistenceUnit = new EntityFrameworkPersistenceUnit<MyDbContext>(
    "MyUnit", 
    dbContextOptions, 
    loggerFactory);

// Register aggregate classes
persistenceUnit
    .AddClass<Event, Guid>()
    .AddClass<Speaker, Guid>()
    .AddClass<Session, Guid>();

// Add to factory
factory.AddPersistenceUnit(persistenceUnit);

// Use in application
using var dataContext = factory.OpenDataContext();
using var transaction = dataContext.BeginTransaction();

var repository = new EventRepository(dataContext);
var myEvent = await repository.LoadAsync(eventId);

// Modify entity
myEvent.ChangeInformation("New Name", "New Description", newPeriod);

// Save changes
dataContext.SaveChanges();
transaction.Commit();
~~~

## Persistence State

ECO tracks the state of each entity within the persistence context:

- **Unknown**: The state cannot be determined
- **Transient**: The entity is new and not yet persisted
- **Persistent**: The entity exists in the database
- **Detached**: The entity was loaded but is no longer tracked

## Listeners

You can register listeners to hook into the persistence unit lifecycle:

~~~ c#
public interface IPersistenceUnitListener
{
    void ContextPreCreate(IPersistenceUnit unit);
    void ContextPostCreate(IPersistenceUnit unit, IPersistenceContext context);
}

// Register a listener
persistenceUnit.AddUnitListener(new MyPersistenceListener());
~~~

This architecture provides a clean separation between your domain model and the persistence infrastructure, allowing you to switch between different providers without changing your business logic.

Go to the next step: [Providers](https://github.com/dogcane/ECO/blob/master/docs/Providers.md) or go to the [Summary](https://github.com/dogcane/ECO/blob/master/docs/Summary.md) of the docs.