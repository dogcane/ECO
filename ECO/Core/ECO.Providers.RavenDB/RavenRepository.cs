using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Raven;
using Raven.Client;

using ECO;
using ECO.Data;
using System.Threading.Tasks;

namespace ECO.Providers.RavenDB
{
    public class RavenRepository<T, K> : RavenReadOnlyRepository<T, K>, IRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region IRepository<T> Membri di

        public void Add(T item)
        {
            AddAsync(item).RunSynchronously();
        }

        public async Task AddAsync(T item)
        {
            await GetCurrentSession().StoreAsync(item);
        }

        public void Update(T item)
        {
            UpdateAsync(item).RunSynchronously();
        }

        public async Task UpdateAsync(T item)
        {
            await GetCurrentSession().StoreAsync(item);
        }

        public void Remove(T item)
        {
            GetCurrentSession().Delete(item);
        }

        public async Task RemoveAsync(T item)
        {
            await Task.Run(() => Remove(item));
        }

        #endregion
    }
}
