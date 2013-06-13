using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Raven;
using Raven.Client;

using ECO;
using ECO.Data;

namespace ECO.Providers.RavenDB
{
    public class RavenRepository<T, K> : RavenReadOnlyRepository<T, K>, IRepository<T, K>
        where T : IAggregateRoot<K>
    {
        #region IRepository<T> Membri di

        public void Add(T item)
        {
            GetCurrentSession().Store(item);
        }

        public void Update(T item)
        {
            GetCurrentSession().Store(item);
        }

        public void Remove(T item)
        {
            GetCurrentSession().Delete(item);
        }

        #endregion
    }
}
