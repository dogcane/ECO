using ECO.Data;
using System.Threading.Tasks;

namespace ECO.Providers.NHibernate
{
    public class NHRepository<T, K> : NHReadOnlyRepository<T, K>, IRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Ctor
        public NHRepository(IDataContext dataContext) : base(dataContext)
        {
        }
        #endregion

        #region IRepository<T> Membri di

        public void Add(T item) => GetCurrentSession().Save(item);

        public async Task AddAsync(T item) => await GetCurrentSession().SaveAsync(item);

        public void Update(T item) => GetCurrentSession().Update(item); //Not necessary with auto-dirty-check

        public async Task UpdateAsync(T item) => await GetCurrentSession().UpdateAsync(item);

        public void Remove(T item) => GetCurrentSession().Delete(item);

        public async Task RemoveAsync(T item) => await GetCurrentSession().DeleteAsync(item);

        #endregion
    }
}
