using ECO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECO.Providers.NHibernate
{
    public class NHReadOnlyRepository<T, K> : NHPersistenceManager<T, K>, IReadOnlyRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Ctor
        public NHReadOnlyRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        #endregion

        #region IReadOnlyEntityManager<T,K> Members

        public virtual T Load(K identity) => GetCurrentSession().Get<T>(identity);

        public virtual async Task<T> LoadAsync(K identity) => await GetCurrentSession().GetAsync<T>(identity);

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator() => GetCurrentSession().Query<T>().GetEnumerator();

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetCurrentSession().Query<T>().GetEnumerator();

        #endregion

        #region IQueryable Members

        public Type ElementType => GetCurrentSession().Query<T>().ElementType;

        public System.Linq.Expressions.Expression Expression => GetCurrentSession().Query<T>().Expression;

        public IQueryProvider Provider => GetCurrentSession().Query<T>().Provider;

        #endregion
    }
}
