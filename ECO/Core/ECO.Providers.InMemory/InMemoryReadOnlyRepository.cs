using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO;

namespace ECO.Providers.InMemory
{
    public class InMemoryReadOnlyRepository<T, K> : InMemoryPersistenceManager<T,K>, IReadOnlyRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region IReadOnlyRepository<T,K> Membri di

        public T Load(K identity)
        {
            if (typeof(K).IsClass && identity == null) return default(T);
            T entity = default(T);
            GetIdentityMap().TryGetValue(identity, out entity);
            return entity;
        }

        public async Task<T> LoadAsync(K identity)
        {
            return await Task.Run(() => Load(identity));
        }

        #endregion

        #region IEnumerable<T> Membri di

        public IEnumerator<T> GetEnumerator()
        {
            return GetEntitySet().GetEnumerator();
        }

        #endregion

        #region IEnumerable Membri di

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEntitySet().GetEnumerator();
        }

        #endregion

        #region IQueryable Membri di

        public Type ElementType
        {
            get { return GetEntitySet().AsQueryable().ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return GetEntitySet().AsQueryable().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return GetEntitySet().AsQueryable().Provider; }
        }

        #endregion
    }
}
