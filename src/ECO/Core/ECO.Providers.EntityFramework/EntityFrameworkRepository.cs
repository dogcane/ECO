using ECO.Data;
using System.Collections.Generic;
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

        public void Add(T item) => DbContext.Set<T>().Add(item);

        public async Task AddAsync(T item) => await DbContext.Set<T>().AddAsync(item);

        public void Update(T item) => DbContext.Set<T>().Update(item);

        public async Task UpdateAsync(T item) => await Task.Run(() => Update(item));

        public void Remove(T item) => DbContext.Set<T>().Remove(item);

        public async Task RemoveAsync(T item) => await Task.Run(() => Remove(item));

        #endregion
    }
}
