using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Raven;
using Raven.Client;

using ECO;
using ECO.Data;

namespace ECO.Providers.RavenDB
{
    public class RavenReadOnlyRepository<T, K> : RavenPersistenceManagerBase<T,K>, IReadOnlyRepository<T,K>
        where T : IAggregateRoot<K>
    {
        #region IReadOnlyRepository Members

        public T Load(K identity)
        {
            return LoadAsync(identity).Result;
        }

        public async Task<T> LoadAsync(K identity)
        {
            return await GetCurrentSession().LoadAsync<T>(identity.ToString());
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

        #region IQueryable<T> Members

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return GetCurrentSession().Query<T>().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return GetCurrentSession().Query<T>().Provider; }
        }

        #endregion
    }
}
