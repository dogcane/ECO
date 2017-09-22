using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using ECO;
using ECO.Data;

using nh = NHibernate;
using nhl = NHibernate.Linq;
using nhm = NHibernate.Metadata;

namespace ECO.Providers.NHibernate
{
    public class NHRepository<T, K> : NHReadOnlyRepository<T, K>, IRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region IRepository<T> Membri di

        public virtual void Add(T item)
        {
            GetCurrentSession().Save(item);
        }

        public virtual async Task AddAsync(T item)
        {
            await Task.Run(() => Add(item));
        }

        public virtual void Update(T item)
        {
            GetCurrentSession().Update(item); //Not necessary with auto-dirty-check
        }

        public virtual async Task UpdateAsync(T item)
        {
            await Task.Run(() => Update(item));
        }

        public virtual void Remove(T item)
        {
            GetCurrentSession().Delete(item);
        }

        public virtual async Task RemoveAsync(T item)
        {
            await Task.Run(() => Remove(item));
        }

        #endregion
    }
}
