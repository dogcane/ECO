using ECO.Data;
using ECO.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Providers.Marten
{
    public class MartenESRepository<T, K> : MartenPersistenceManager<T, K>, IESRepository<T, K>
        where T : class, IESAggregateRoot<K>
    {
        #region Ctor

        public MartenESRepository(IDataContext dataContext) : base(dataContext)
        {

        }

        #endregion

        #region IESRepository<T, K> Members

        public T? Load(K identity) => typeof(K).Name switch
        {
            nameof(String) => GetCurrentSession().Events.AggregateStream<T>(Convert.ToString(identity)),
            nameof(Guid) => GetCurrentSession().Events.AggregateStream<T>(Guid.Parse(Convert.ToString(identity))),
            _ => throw new InvalidOperationException()
        };

        public async Task<T?> LoadAsync(K identity) => typeof(K).Name switch
        {
            nameof(String) => await GetCurrentSession().Events.AggregateStreamAsync<T>(Convert.ToString(identity)),
            nameof(Guid) => await GetCurrentSession().Events.AggregateStreamAsync<T>(Guid.Parse(Convert.ToString(identity))),
            _ => throw new InvalidOperationException()
        };

        public IEnumerable<dynamic> LoadEvents(K identity) => typeof(K).Name switch
        {
            nameof(String) => GetCurrentSession().Events.FetchStream(Convert.ToString(identity)).Select(@event => @event.Data),
            nameof(Guid) => GetCurrentSession().Events.FetchStream(Guid.Parse(Convert.ToString(identity))).Select(@event => @event.Data),
            _ => throw new InvalidOperationException()
        };

        public async Task<IEnumerable<dynamic>> LoadEventsAsync(K identity) => typeof(K).Name switch
        {
            nameof(String) => (await GetCurrentSession().Events.FetchStreamAsync(Convert.ToString(identity))).Select(@event => @event.Data),
            nameof(Guid) => (await GetCurrentSession().Events.FetchStreamAsync(Guid.Parse(Convert.ToString(identity)))).Select(@event => @event.Data),
            _ => throw new InvalidOperationException()
        };

        public void Save(T item)
        {
            var aggregate = item as IESAggregateRoot<K>;            
            var events = aggregate!.GetUncommittedEvents().ToArray();
            var session = GetCurrentSession();
            if (typeof(K).Name == nameof(Guid))
                session.Events.Append(Guid.Parse(Convert.ToString(aggregate.Identity)), aggregate.Version, events);
            else if (typeof(K).Name == nameof(String))
                session.Events.Append(Convert.ToString(aggregate.Identity), aggregate.Version, events);
            else
                throw new InvalidOperationException();
            GetCurrentSession().SaveChanges();
            aggregate.ClearUncommittedEvents();
        }

        public async Task SaveAsync(T item)
        {
            var aggregate = item as IESAggregateRoot<K>;
            var events = aggregate!.GetUncommittedEvents().ToArray();
            var session = GetCurrentSession();
            if (typeof(K).Name == nameof(Guid))
                session.Events.Append(Guid.Parse(Convert.ToString(aggregate.Identity)), aggregate.Version, events);
            else if (typeof(K).Name == nameof(String))
                session.Events.Append(Convert.ToString(aggregate.Identity), aggregate.Version, events);
            else
                throw new InvalidOperationException();
            await GetCurrentSession().SaveChangesAsync();
            aggregate.ClearUncommittedEvents();
        }

        #endregion
    }
}
