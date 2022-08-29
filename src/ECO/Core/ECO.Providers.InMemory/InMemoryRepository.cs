using ECO.Data;
using System.Threading.Tasks;

namespace ECO.Providers.InMemory
{
    public class InMemoryRepository<T, K> : InMemoryReadOnlyRepository<T, K>, IRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Ctor

        public InMemoryRepository(IDataContext dataContext)
            : base(dataContext)
        {

        }

        #endregion

        #region IRepository<T> Membri di

        public virtual void Add(T item) => _EntitySet.TryAdd(item.Identity, item);

        public virtual async Task AddAsync(T item) => await Task.Run(() => Add(item));

        public virtual void Update(T item) => _EntitySet.TryUpdate(item.Identity, item, _EntitySet[item.Identity]);

        public virtual async Task UpdateAsync(T item) => await Task.Run(() => Update(item));

        public virtual void Remove(T item) => _EntitySet.TryRemove(item.Identity, out _);

        public virtual async Task RemoveAsync(T item) => await Task.Run(() => Remove(item));

        #endregion
    }
}
