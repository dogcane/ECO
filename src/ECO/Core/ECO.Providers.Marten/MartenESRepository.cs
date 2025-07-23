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

    public async ValueTask<T?> LoadAsync(K identity) => GetIdentityType(identity) switch
    {
        IdentityType.String => await DocumentSession.Events.AggregateStreamAsync<T>(Convert.ToString(identity)!),
        IdentityType.Guid => await DocumentSession.Events.AggregateStreamAsync<T>(Guid.Parse(Convert.ToString(identity)!)),
        _ => throw new ArgumentOutOfRangeException(nameof(identity), $"Unsupported identity type: {typeof(K).Name}")
    };

    public IEnumerable<dynamic> LoadEvents(K identity) => LoadEventsAsync(identity).GetAwaiter().GetResult();

    public async Task<IEnumerable<dynamic>> LoadEventsAsync(K identity) => GetIdentityType(identity) switch
    {
        IdentityType.String => (await DocumentSession.Events.FetchStreamAsync(Convert.ToString(identity)!)).Select(@event => @event.Data),
        IdentityType.Guid => (await DocumentSession.Events.FetchStreamAsync(Guid.Parse(Convert.ToString(identity)!))).Select(@event => @event.Data),
        _ => throw new ArgumentOutOfRangeException(nameof(identity), $"Unsupported identity type: {typeof(K).Name}")
    };

    public void Save(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        
        var aggregate = (IESAggregateRoot<K>)item;
        var events = aggregate.GetUncommittedEvents().ToArray();
        var session = DocumentSession;

        switch (GetIdentityType(aggregate.Identity!))
        {
            case IdentityType.Guid:
                session.Events.Append(Guid.Parse(Convert.ToString(aggregate.Identity)!), aggregate.Version, events);
                break;
            case IdentityType.String:
                session.Events.Append(Convert.ToString(aggregate.Identity)!, aggregate.Version, events);
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
        
        switch (GetIdentityType(aggregate.Identity!))
        {
            case IdentityType.Guid:
                session.Events.Append(Guid.Parse(Convert.ToString(aggregate.Identity)!), aggregate.Version, events);
                break;
            case IdentityType.String:
                session.Events.Append(Convert.ToString(aggregate.Identity)!, aggregate.Version, events);
                break;
            default:
                throw new InvalidOperationException($"Unsupported identity type: {typeof(K).Name}");
        }
        
        await PersistenceContext.SaveChangesAsync();
        aggregate.ClearUncommittedEvents();
    }

    #endregion

    #region Private Methods

    private static IdentityType GetIdentityType(K identity) => identity switch
    {
        string => IdentityType.String,
        Guid => IdentityType.Guid,
        _ => Type.GetTypeCode(typeof(K)) switch
        {
            TypeCode.String => IdentityType.String,
            TypeCode.Object when typeof(K) == typeof(Guid) => IdentityType.Guid,
            _ => IdentityType.Unsupported
        }
    };

    private enum IdentityType
    {
        String,
        Guid,
        Unsupported
    }

    #endregion
}
