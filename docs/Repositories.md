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
