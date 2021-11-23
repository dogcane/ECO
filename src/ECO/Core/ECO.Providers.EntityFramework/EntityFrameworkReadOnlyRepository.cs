using ECO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECO.Providers.EntityFramework
{
    public class EntityFrameworkReadOnlyRepository<T, K> : EntityFrameworkPersistenceManager<T, K>, IReadOnlyRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Ctor

        public EntityFrameworkReadOnlyRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        #endregion

        #region IReadOnlyRepository<T,K> Members

        public T Load(K identity) => DbContext.Set<T>().Find(identity);

        public async Task<T> LoadAsync(K identity) => await DbContext.Set<T>().FindAsync(identity);

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator() => DbContext.Set<T>().AsEnumerable().GetEnumerator();

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion

        #region IQueryable Members

        public Type ElementType => DbContext.Set<T>().AsQueryable().ElementType;

        public System.Linq.Expressions.Expression Expression => DbContext.Set<T>().AsQueryable().Expression;

        public IQueryProvider Provider => DbContext.Set<T>().AsQueryable().Provider;

        #endregion
    }
}
