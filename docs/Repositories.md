# Repositories

This is a "standard" implementation of the [repository pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design#:~:text=The%20Repository%20pattern%20Repositories%20are%20classes%20or%20components,to%20access%20databases%20from%20the%20domain%20model%20layer.):

Every aggregate root can define its repository kind. ECO allows two kinds of repositories:

- Read only repositories (for load and search methods) => IReadOnlyRepository\<T,K\>
- "Full" repositories (with all CRUD methods) => IRepository\<T,K\>

## Read only repository

This is the base repository interface (also the "full" repository implements this interface) :

~~~ c#
public interface IReadOnlyRepository<T, K> : IQueryable<T>, IPersistenceManager<T, K>
    where T : class, IAggregateRoot<K>
{
    T Load(K identity);
    Task<T> LoadAsync(K identity);
}

~~~

The interface implements :

- IQueryable\<T\> => it support all linq stuff (managed by the provider, such as ECO.Providers.EntityFramework)
- IPersistenceManager\<T, K\> => it gives access to the related persistence context

These constraint are also defined :

- T => it must be a class and it must implement IAggregateRoot\<K\>
- K => it's the type of aggregate's identifier

It defines the following methods :

- Load/LoadAsync => method for loading a single entity/aggregate root by identity

## Repository

This is the "full version" repository interface :

~~~ c#
public interface IRepository<T, K> : IReadOnlyRepository<T, K>
        where T : class, IAggregateRoot<K>
{
    void Add(T item);
    Task AddAsync(T item);
    void Update(T item);
    Task UpdateAsync(T item);
    void Remove(T item);
    Task RemoveAsync(T item);
}
~~~

The interface implements :

- IReadOnlyRepository\<T, K\> => it support all "read only repository" stuff

These constraint are also defined :

- T => it must be a class and it must implement IAggregateRoot\<K\>
- K => it's the type of aggregate's identifier

It defines the following methods :

- Add/AddAsync => method for adding a single aggregate/entity
- Update/UpdateAsync => method for updating a single aggregate/entity
- Remove/RemoveAsync => method for removing a single aggregate/entity

## Async streaming

Starting from ECO 4.1 every repository can stream aggregate roots asynchronously, so large result sets can be consumed without materializing the whole collection:

~~~ c#
await foreach (var evt in _EventRepository.ToAsyncEnumerable(cancellationToken))
{
    // process the aggregate
}
~~~

`ToAsyncEnumerable` is an extension method defined in `ReadOnlyRepositoryExtensions`, available to both `IReadOnlyRepository<T, K>` and `IRepository<T, K>`:

~~~ c#
public static IAsyncEnumerable<T> ToAsyncEnumerable<T, K>(
    this IReadOnlyRepository<T, K> repository,
    CancellationToken cancellationToken = default)
    where T : class, IAggregateRoot<K>
    where K : notnull, IEquatable<K>
~~~

The repository interfaces are unchanged: the stream is provided by the capability interface `IAsyncEnumerableSource<T>`, which ECO providers implement:

~~~ c#
public interface IAsyncEnumerableSource<T>
{
    IAsyncEnumerable<T> ToAsyncEnumerable(CancellationToken cancellationToken = default);
}
~~~

`ToAsyncEnumerable` dispatches to the capability when the repository implements it, and otherwise falls back to a synchronous enumeration of the repository wrapped in an asynchronous iterator. This keeps custom repositories written against ECO 4.0 source-compatible, and it means a custom repository automatically gets the API even without implementing the capability.

### Provider support

| Provider | Implementation | Notes |
|---|---|---|
| Entity Framework Core | `AsAsyncEnumerable` | True streaming, server-side cursor; change tracking applies as configured |
| Marten | `QueryableExtensions.ToAsyncEnumerable` | True streaming |
| MongoDB | `MongoQueryable.ToCursorAsync` + `MoveNextAsync` | Streams one cursor batch at a time; the cursor is disposed when the enumeration ends or is cancelled |
| NHibernate | `ToListAsync` + `yield` | NHibernate has no native async streaming: the full result is materialized before the first item is returned |
| In Memory | snapshot of the static dictionary | Enumeration works on a snapshot: items added after the stream starts are not included |

### Composed queries

`ToAsyncEnumerable` streams the whole aggregate set of the repository. Query operators such as `Where`, `Select` or `OrderBy` return an `IQueryable<T>`, so for filtered or projected results you can either filter before materializing or use the provider native API:

~~~ c#
// Entity Framework Core
await foreach (var evt in _EventRepository
    .Where(e => e.Period.StartDate >= from)
    .AsAsyncEnumerable()
    .WithCancellation(cancellationToken))
{
    ...
}

// Marten
await foreach (var evt in DocumentSession.Query<Event>()
    .Where(e => e.Period.StartDate >= from)
    .ToAsyncEnumerable(cancellationToken))
{
    ...
}

// MongoDB
using var cursor = await _EventRepository
    .Where(e => e.Period.StartDate >= from)
    .ToCursorAsync(cancellationToken);
~~~

For NHibernate use `ToListAsync(cancellationToken)` on the filtered query.

### Lifetime and correctness

- The stream is lazy: the query runs when enumeration starts, and the persistence context (EF Core `DbContext`, NHibernate `ISession`, Marten `IDocumentSession`, Mongo collection) must stay alive for the whole enumeration.
- Never return an `IAsyncEnumerable<T>` from a method that owns a scoped `DbContext`/session: the context is disposed when the method returns, and the consumer enumerates a dead stream. Enumerate inside the scope or materialize with `ToListAsync` before returning.
- A streaming EF Core query keeps the `DbContext` busy: do not start other operations on the same context while enumerating. Use a no-tracking query if change tracking is not needed.
- Always pass a `CancellationToken`: it is observed by every provider implementation and by the synchronous fallback, and it also disposes the MongoDB cursor.
- Enumeration is not thread-safe; do not consume the same stream from multiple tasks.

### Testing with mocks

`SetupReadOnlyRepository` and `SetupRepository` in `ECO.Integrations.Moq` also configure `IAsyncEnumerableSource<T>`, so a mocked repository streams the configured list:

~~~ c#
var repository = new Mock<IEventRepository>();
repository.SetupRepository<Event, Guid>(events);
await foreach (var evt in repository.Object.ToAsyncEnumerable())
{
    ...
}
~~~

If the mock only sets up `IQueryable<T>` (for example with `SetupQueryable`), the extension transparently uses the synchronous fallback.

> **.NET 10 note:** `System.Linq.AsyncEnumerable` is part of the framework, so the `System.Linq.Async` package is no longer required for LINQ operators over `IAsyncEnumerable<T>`. Remove it (or update it) to avoid duplicate extension methods.

## Implementation of a repository in applications

In your applications you have two ways of implementing a repository with ECO :

- Use a generic repository
- Define a typed repository

### Use a generic repository

This is the simplest way to define a repository with ECO. You don't need to write code or classes, the only thing you need it's to configure the generic repository into your IoC container.

### Define a typed repository (the preferred way)

This is the preferred way to work with repositories and ECO. The first step is to defined a custom repository interface :

~~~ c#
public interface IEventRepository : IRepository<Event, Guid>
{

}
~~~

And then you have to implement the interface with a "fully-provider" repository class. In our example we use the "In Memory" provider :

~~~ c#
public class EventMemoryRepository : InMemoryRepository<Event, Guid>, IEventRepository
{
    public EventMemoryRepository(IDataContext dataContext)
        : base(dataContext)
    {

    }
}
~~~

After that you can register the new class with your IoC container and you can start using it. This is the preferred way for many reasons :

- A typed interface it's clearer than the "generic" version

- You can override the methods of the base implementations of the standard providers

~~~ c#
public class EventEFRepository : EntityFrameworkRepository<Event, Guid>, IEventRepository
{
...
    public override void Update(Event item)
    {    
        ...
        base.Update(item);
    }
...
}
~~~

- You can define custom methods and implement they into the typed repository (using provider specific functionalities)

~~~ c#
public interface IEventRepository : IRepository<Event, Guid>
{
    Task<int> GetAllEventsCountBySpeaker(Speaker speaker);
}

public class EventNHRepository : NHRepository<Event, Guid>, IEventRepository
{
    public async Task<int> GetAllEventsCountBySpeaker(Speaker speaker)
    {
        return await GetCurrentSession()
            .QueryOver<Event>()
            .Where(ev => ev.Sessions.Any(s => s.Speaker == speaker))
            .RowCountAsync();
    }
}
~~~

Go to the next step: [Persistence units and contexts](Units-Contexts.md) or go to the [Summary](Summary.md) of the docs.
