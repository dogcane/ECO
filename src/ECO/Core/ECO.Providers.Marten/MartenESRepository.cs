namespace ECO.Providers.Marten;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECO.Data;
using ECO.EventSourcing;

public class MartenESRepository<T, K>(IDataContext dataContext) : MartenPersistenceManager<T, K>(dataContext), IESRepository<T, K>
    where T : class, IESAggregateRoot<K>
{
    #region IESRepository<T, K> Members

    public T? Load(K identity) => LoadAsync(identity).GetAwaiter().GetResult();

    public async ValueTask<T?> LoadAsync(K identity) => identity switch
    {
        string stringId => await DocumentSession.Events.AggregateStreamAsync<T>(stringId),
        Guid guidId => await DocumentSession.Events.AggregateStreamAsync<T>(guidId),
        _ => throw new ArgumentOutOfRangeException(nameof(identity), $"Unsupported identity type: {typeof(K).Name}")
    };

    public IEnumerable<dynamic> LoadEvents(K identity) => LoadEventsAsync(identity).GetAwaiter().GetResult();

    public async Task<IEnumerable<dynamic>> LoadEventsAsync(K identity) => identity switch
    {
        string stringId => (await DocumentSession.Events.FetchStreamAsync(stringId)).Select(@event => @event.Data),
        Guid guidId => (await DocumentSession.Events.FetchStreamAsync(guidId)).Select(@event => @event.Data),
        _ => throw new ArgumentOutOfRangeException(nameof(identity), $"Unsupported identity type: {typeof(K).Name}")
    };

    public void Save(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        
        var aggregate = (IESAggregateRoot<K>)item;
        var events = aggregate.GetUncommittedEvents().ToArray();
        var session = DocumentSession;

        switch (aggregate.Identity)
        {
            case Guid guidId:
                session.Events.Append(guidId, aggregate.Version, events);
                break;
            case string stringId:
                session.Events.Append(stringId, aggregate.Version, events);
                break;
            default:
                throw new InvalidOperationException($"Unsupported identity type: {typeof(K).Name}");
        }
        
        PersistenceContext.SaveChanges();
        aggregate.ClearUncommittedEvents();
    }

    public async Task SaveAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        
        var aggregate = (IESAggregateRoot<K>)item;
        var events = aggregate.GetUncommittedEvents().ToArray();
        var session = DocumentSession;
        
        switch (aggregate.Identity)
        {
            case Guid guidId:
                session.Events.Append(guidId, aggregate.Version, events);
                break;
            case string stringId:
                session.Events.Append(stringId, aggregate.Version, events);
                break;
            default:
                throw new InvalidOperationException($"Unsupported identity type: {typeof(K).Name}");
        }
        
        await PersistenceContext.SaveChangesAsync();
        aggregate.ClearUncommittedEvents();
    }

    #endregion
}
