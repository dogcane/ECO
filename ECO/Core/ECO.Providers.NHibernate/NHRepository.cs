using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using ECO;
using ECO.Data;

using nh = NHibernate;
using nhl = NHibernate.Linq;
using nhm = NHibernate.Metadata;

namespace ECO.Providers.NHibernate
{
    public abstract class NHRepository<T, K> : NHReadOnlyRepository<T, K>, IRepository<T, K>
        where T : IAggregateRoot<K>
    {
        #region IRepository<T> Membri di

        public void Add(T item)
        {
            GetCurrentSession().Save(item);
        }

        public void Update(T item)
        {
            GetCurrentSession().Update(item); //Not necessary with auto-dirty-check
        }

        public void Remove(T item)
        {
            GetCurrentSession().Delete(item);
        }

        #endregion
    }
}
