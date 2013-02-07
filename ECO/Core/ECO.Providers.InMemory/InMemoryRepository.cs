using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;

namespace ECO.Providers.InMemory
{
    public class InMemoryRepository<T, K> : InMemoryReadOnlyRepository<T, K>, IRepository<T, K>
        where T : AggregateRoot<K>
    {
        #region IRepository<T> Membri di

        public void Add(T item)
        {
            GetIdentityMap().Add(item.Identity, item);
            GetEntitySet().Add(item);
        }
        public void Remove(T item)
        {
            GetIdentityMap().Remove(item.Identity);
            GetEntitySet().Remove(item);
        }

        #endregion
    }
}
