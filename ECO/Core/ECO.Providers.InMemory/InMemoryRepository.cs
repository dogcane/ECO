using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECO;

namespace ECO.Providers.InMemory
{
    public class InMemoryRepository<T, K> : InMemoryReadOnlyRepository<T, K>, IRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region IRepository<T> Membri di

        public void Add(T item)
        {
            GetIdentityMap().Add(item.Identity, item);
            GetEntitySet().Add(item);
        }

        public async Task AddAsync(T item)
        {
            await Task.Run(() => Add(item));
        }

        public void Update(T item)
        {
            //NOTHING
        }

        public async Task UpdateAsync(T item)
        {
            await Task.Run(() => Update(item));
        }

        public void Remove(T item)
        {
            GetIdentityMap().Remove(item.Identity);
            GetEntitySet().Remove(item);
        }

        public async Task RemoveAsync (T item)
        {
            await Task.Run(() => Remove(item));
        }

        #endregion
    }
}
