using ECO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECO.Providers.InMemory
{
    public class InMemoryReadOnlyRepository<T, K> : InMemoryPersistenceManager<T, K>, IReadOnlyRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Ctor

        public InMemoryReadOnlyRepository(IDataContext dataContext)
            : base(dataContext)
        {

        }

        #endregion

        #region IReadOnlyRepository<T,K> Membri di

        public virtual T Load(K identity)
        {
            lock (_SycnLock)
            {
                _IdentityMap.TryGetValue(identity, out T entity);
                return entity;
            }
        }

        public virtual async Task<T> LoadAsync(K identity)
        {
            return await Task.Run(() => Load(identity));
        }

        #endregion

        #region IEnumerable<T> Membri di

        public virtual IEnumerator<T> GetEnumerator()
        {
            lock (_SycnLock)
            {
                return _EntitySet.ToList().GetEnumerator();
            }
        }

        #endregion

        #region IEnumerable Membri di

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            lock (_SycnLock)
            {
                return _EntitySet.ToList().GetEnumerator();
            }
        }

        #endregion

        #region IQueryable Membri di

        public virtual Type ElementType
        {
            get { return _EntitySet.AsQueryable().ElementType; }
        }

        public virtual System.Linq.Expressions.Expression Expression
        {
            get { return _EntitySet.AsQueryable().Expression; }
        }

        public virtual IQueryProvider Provider
        {
            get { return _EntitySet.AsQueryable().Provider; }
        }

        #endregion
    }
}
