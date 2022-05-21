using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Providers.EntityFramework
{
    public class EntityFrameworkRepository<T, K> : EntityFrameworkReadOnlyRepository<T, K>, IRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region IRepository<T,K> Members

        public void Add(T item)
        {
            GetCurrentDbContext().Set<T>().Add(item);
        }

        public async Task AddAsync(T item)
        {
            await Task.Run(() => Add(item));
        }

        public void Update(T item)
        {
            GetCurrentDbContext().Entry<T>(item).State = EntityState.Modified;
        }

        public async Task UpdateAsync(T item)
        {
            await Task.Run(() => Update(item));
        }

        public void Remove(T item)
        {
            GetCurrentDbContext().Set<T>().Remove(item);
        }

        public async Task RemoveAsync(T item)
        {
            await Task.Run(() => Remove(item));
        }

        #endregion
    }
}
