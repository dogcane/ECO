using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Providers.EntityFramework
{
    public class EntityFrameworkReadOnlyRepository<T, K> : EntityFrameworkPersistenceManager<T, K>, IReadOnlyRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region IReadOnlyRepository<T,K> Members

        public T Load(K identity)
        {
            if (typeof(K).IsClass && identity == null) return default(T);
            return GetCurrentDbContext().Set<T>().Find(identity);
        }

        public async Task<T> LoadAsync(K identity)
        {
            if (typeof(K).IsClass && identity == null) return default(T);
            return await GetCurrentDbContext().Set<T>().FindAsync(identity);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return GetCurrentDbContext().Set<T>().AsEnumerable().GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IQueryable Members

        public Type ElementType
        {
            get { return GetCurrentDbContext().Set<T>().AsQueryable().ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return GetCurrentDbContext().Set<T>().AsQueryable().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return GetCurrentDbContext().Set<T>().AsQueryable().Provider; }
        }

        #endregion
    }
}
