using ECO.Data;
using ECO.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECO.Providers.Marten;

public class MartenESRepository<T, K>(IDataContext dataContext) : MartenPersistenceManager<T, K>(dataContext), IESRepository<T, K>
    where T : class, IESAggregateRoot<K>
{
    #region IESRepository<T, K> Members

    public T? Load(K identity) => typeof(K).Name switch
    {
        nameof(String) => DocumentSession.Events.AggregateStream<T>(Convert.ToString(identity)),
        nameof(Guid) => DocumentSession.Events.AggregateStream<T>(Guid.Parse(Convert.ToString(identity))),
        _ => throw new ArgumentOutOfRangeException(nameof(identity))
    };

    public async Task<T?> LoadAsync(K identity) => typeof(K).Name switch
    {
        nameof(String) => await DocumentSession.Events.AggregateStreamAsync<T>(Convert.ToString(identity)),
        nameof(Guid) => await DocumentSession.Events.AggregateStreamAsync<T>(Guid.Parse(Convert.ToString(identity))),
        _ => throw new ArgumentOutOfRangeException(nameof(identity))
    };

    public IEnumerable<dynamic> LoadEvents(K identity) => typeof(K).Name switch
    {
        nameof(String) => DocumentSession.Events.FetchStream(Convert.ToString(identity)).Select(@event => @event.Data),
        nameof(Guid) => DocumentSession.Events.FetchStream(Guid.Parse(Convert.ToString(identity))).Select(@event => @event.Data),
        _ => throw new ArgumentOutOfRangeException(nameof(identity))
    };

    public async Task<IEnumerable<dynamic>> LoadEventsAsync(K identity) => typeof(K).Name switch
    {
        nameof(String) => (await DocumentSession.Events.FetchStreamAsync(Convert.ToString(identity))).Select(@event => @event.Data),
        nameof(Guid) => (await DocumentSession.Events.FetchStreamAsync(Guid.Parse(Convert.ToString(identity)))).Select(@event => @event.Data),
        _ => throw new ArgumentOutOfRangeException(nameof(identity))
    };

    public void Save(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        var aggregate = (IESAggregateRoot<K>)item;
        var events = aggregate!.GetUncommittedEvents().ToArray();
        var session = DocumentSession;
        if (typeof(K).Name == nameof(Guid))
            session.Events.Append(Guid.Parse(Convert.ToString(aggregate.Identity)), aggregate.Version, events);
        else if (typeof(K).Name == nameof(String))
            session.Events.Append(Convert.ToString(aggregate.Identity), aggregate.Version, events);
        else
            throw new InvalidOperationException();
        DocumentSession.SaveChanges();
        aggregate.ClearUncommittedEvents();
    }

    public async Task SaveAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        var aggregate = (IESAggregateRoot<K>)item;
        var events = aggregate!.GetUncommittedEvents().ToArray();
        var session = DocumentSession;
        if (typeof(K).Name == nameof(Guid))
            session.Events.Append(Guid.Parse(Convert.ToString(aggregate.Identity)), aggregate.Version, events);
        else if (typeof(K).Name == nameof(String))
            session.Events.Append(Convert.ToString(aggregate.Identity), aggregate.Version, events);
        else
            throw new InvalidOperationException();
        await DocumentSession.SaveChangesAsync();
        aggregate.ClearUncommittedEvents();
    }

    #endregion
}
