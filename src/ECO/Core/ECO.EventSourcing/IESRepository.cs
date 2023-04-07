using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECO.EventSourcing
{
    public interface IESRepository<T, K> : IRepository<T, K>
        where T : class, IESAggregateRoot<K>
    {
        Task<T> LoadStream(K identity);

        Task<IEnumerable<IESEvent>> LoadEvents(K identity);
    }
}
