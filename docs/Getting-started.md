# Getting started guide

## Why use ECO

The task of creating "base repositories" or the management of a generic data context (es EFCore) is always a recurring and tedius task.
The main goal of ECO is to simplify all the "infrastructure" of this management and allows the developer to work faster in a DDD perspective.

## How do I use ECO?

The first step to work with ECO is the definition of the model (entities, aggregates and so on).

In the ECO.SampleApp there is a simple context based on a "community event tool" for the organization of community events:

- you have the concept of community events with its sessions
- you have the concept of speakers, who hold the sessions

If we take a look at the "Event" entity:

- It's an aggregate root
- It contains properties and its behaviour
- It contains a collection of related entities, its sessions

From a "c#" perspective:

- It's a common POCO class
- It derives from AggregateRoot\<T\> (the key type is a Guid in this case) and it can be load with a dedicated repository

~~~ c#

public class Event : AggregateRoot<Guid>
{
    ...
    public virtual string Name { get; protected set; }
    public virtual string Description { get; protected set; }
    public virtual Period Period { get; protected set; }
    public virtual IEnumerable<Session> Sessions => _Sessions;
    ...
    public virtual OperationResult ChangeInformation(string name, string description, Period period)
    {
    ...
    }
    ...
    public virtual OperationResult<Session> AddSession(string title, string description, int level, Speaker speaker)
    {
    ...
    }
    ...
    public virtual OperationResult RemoveSession(Session session)
    {
    ...
    }
}

~~~

On the other side, the "Session" entity :

- It's a POCO class
- It's a "nested" entity

From a "c#" perspective:

- It's a common POCO class
- It derives from Entity\<T\> (the key type is a Guid in this case) and it doesn't have a specific repository. It's a part of the "event aggregate"

~~~ c#

public class Session : Entity<Guid>
{
    ...
    public virtual Event Event { get; protected set; }
    public virtual string Title { get; protected set; }
    public virtual string Description { get; protected set; }
    public virtual int Level { get; protected set; }
    public virtual Speaker Speaker { get; protected set; }
    ....
}

~~~

The next step is to defined a "repository" for the aggregate root "event" and implement the "IRepository" generic interface:

~~~ c#
public interface IEventRepository : IRepository<Event, Guid>
{

}
~~~

After that it's important to define the provider that we want to use for ECO and import the relate package. In our case we decided to use the "InMemory" provider (only for tests!!!). The next step is to implement our repository interface:

~~~ c#
public class EventMemoryRepository : InMemoryRepository<Event, Guid>, IEventRepository
{
    public EventMemoryRepository(IDataContext dataContext)
        : base(dataContext)
    {

    }
}
~~~

In the appsettings.json now we need to configure ECO and our persistence context:

~~~ json
...
"eco": {
    "persistenceUnits": [
    {
        "name": "ecosampleapp.inmemory",
        "type": "ECO.Providers.InMemory.InMemoryPersistenceUnit, ECO.Providers.InMemory",
        "classes": [
        "ECO.Sample.Domain.Event, ECO.Sample.Domain",
        "ECO.Sample.Domain.Speaker, ECO.Sample.Domain"
        ]
    }
    ]
}
...
~~~

And the last step is to configure ECO services into AspNet startup:

~~~ c#
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddDataContext(options =>
    {
        options.UsingConfiguration(Configuration);
    });
    services.AddScoped<IEventRepository, EventMemoryRepository>();
    ...
}
~~~

With these commands we:

- Setup all ECO environment
- A "Scoped" DataContext it's created on every request
- We can "inject" a DataContext and our Repositories into our controllers or our services

In our example we use MediatR to manage Commands/Queries Handlers and from a specific handler we can have:

~~~ c#
public async Task<OperationResult<Guid>> Handle(Command request, CancellationToken cancellationToken)
{
    using var transactionContext = await _DataContext.BeginTransactionAsync();
    try
    {
        var eventResult = Event.Create(request.Name, request.Description, new Period(request.StartDate, request.EndDate));
        if (!eventResult.Success)
        {
            return OperationResult<Guid>.MakeFailure(eventResult.TranslateContext("Period.StartDate", "StartDate").TranslateContext("Period.EndDate", "EndDate").Errors);
        }
        await _EventRepository.AddAsync(eventResult.Value);
        await _DataContext.SaveChangesAsync();
        await transactionContext.CommitAsync();
        return OperationResult<Guid>.MakeSuccess(eventResult.Value.Identity);                    
    }
    catch (Exception ex)
    {
        _Logger.LogError(ex, "Error during the execution of the handler");
        return OperationResult.MakeFailure().AppendError("Handle", ex.Message);
    }
}
~~~

Our persistence stack is up & running!

Go to the [Summary](https://github.com/dogcane/ECO/blob/master/docs/Summary.md) of the docs.
