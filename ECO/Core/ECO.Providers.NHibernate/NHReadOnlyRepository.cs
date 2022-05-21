using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using nh = NHibernate;
using nhl = NHibernate.Linq;

using ECO;
using ECO.Data;

namespace ECO.Providers.NHibernate
{
    public class NHReadOnlyRepository<T, K> : NHPersistenceManager<T, K>, IReadOnlyRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region IReadOnlyEntityManager<T,K> Members

        public virtual T Load(K identity)
        {
            if (typeof(K).IsClass && identity == null) return default(T);
            return GetCurrentSession().Get<T>(identity);
        }

        public virtual async Task<T> LoadAsync(K identity)
        {
            //return await Task.Run(() => Load(identity));
            if (typeof(K).IsClass && identity == null) return default(T);
            return await GetCurrentSession().GetAsync<T>(identity);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return GetCurrentSession().Query<T>().AsEnumerable().GetEnumerator();
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
            get { return GetCurrentSession().Query<T>().ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return GetCurrentSession().Query<T>().Expression; }
        }

        public System.Linq.IQueryProvider Provider
        {
            get { return GetCurrentSession().Query<T>().Provider; }
        }

        #endregion
    }
}
