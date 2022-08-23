﻿using ECO.Data;
using System.Threading.Tasks;

namespace ECO.Providers.EntityFramework
{
    public class EntityFrameworkRepository<T, K> : EntityFrameworkReadOnlyRepository<T, K>, IRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Ctor

        public EntityFrameworkRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        #endregion

        #region IRepository<T,K> Members

        public virtual void Add(T item) => DbContext.Set<T>().Add(item);

        public virtual async Task AddAsync(T item) => await Task.Run(() => Add(item));

        public virtual void Update(T item) => DbContext.Set<T>().Update(item);

        public virtual async Task UpdateAsync(T item) => await Task.Run(() => Update(item));

        public virtual void Remove(T item) => DbContext.Set<T>().Remove(item);

        public virtual async Task RemoveAsync(T item) => await Task.Run(() => Remove(item));

        #endregion
    }
}
