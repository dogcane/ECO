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
            return GetCurrentSession().Get<T>(identity);
        }

        public virtual async Task<T> LoadAsync(K identity)
        {
            return await Task.Run(() => Load(identity));
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return nhl.LinqExtensionMethods.Query<T>(GetCurrentSession()).AsEnumerable().GetEnumerator();
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
            get { return nhl.LinqExtensionMethods.Query<T>(GetCurrentSession()).ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return nhl.LinqExtensionMethods.Query<T>(GetCurrentSession()).Expression; }
        }

        public System.Linq.IQueryProvider Provider
        {
            get { return nhl.LinqExtensionMethods.Query<T>(GetCurrentSession()).Provider; }
        }

        #endregion
    }
}
