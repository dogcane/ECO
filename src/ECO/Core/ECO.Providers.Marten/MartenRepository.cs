using ECO.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Providers.Marten
{
    public class MartenRepository<T,K> : MartenReadOnlyRepository<T, K>, IRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Ctor
        public MartenRepository(IDataContext dataContext) : base(dataContext)
        {
        }
        #endregion

        #region IRepository<T> Membri di

        public virtual void Add(T item) => GetCurrentSession().Store(item);

        public virtual async Task AddAsync(T item) => await Task.Run(() => Add(item));

        public virtual void Update(T item) => GetCurrentSession().Update(item);

        public virtual async Task UpdateAsync(T item) => await Task.Run(() => GetCurrentSession().Update(item));

        public virtual void Remove(T item) => GetCurrentSession().Delete(item);

        public virtual async Task RemoveAsync(T item) => await Task.Run(() => GetCurrentSession().Delete(item));

        #endregion
    }
}
