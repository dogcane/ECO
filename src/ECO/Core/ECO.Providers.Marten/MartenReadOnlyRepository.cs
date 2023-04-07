using ECO.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Providers.Marten
{
    public class MartenReadOnlyRepository<T, K> : MartenPersistenceManager<T, K>, IReadOnlyRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Ctor

        public MartenReadOnlyRepository(IDataContext dataContext) : base(dataContext)
        {

        }

        #endregion

        #region IReadOnlyEntityManager<T,K> Members

        public virtual T Load(K identity)
        {
            if (identity == null) throw new InvalidOperationException();
            return typeof(K).Name switch
            {
                nameof(String) => GetCurrentSession().Load<T>(Convert.ToString(identity)),
                nameof(Int32) => GetCurrentSession().Load<T>(Convert.ToInt32(identity)),
                nameof(Int64) => GetCurrentSession().Load<T>(Convert.ToInt64(identity)),
                nameof(Guid) => GetCurrentSession().Load<T>(identity),
                _ => throw new InvalidOperationException()
            };            
            
        }

        public virtual async Task<T> LoadAsync(K identity)
        {
            if (identity == null) throw new InvalidOperationException();
            return typeof(K).Name switch
            {
                nameof(String) => await GetCurrentSession().LoadAsync<T>(Convert.ToString(identity)),
                nameof(Int32) => await GetCurrentSession().LoadAsync<T>(Convert.ToInt32(identity)),
                nameof(Int64) => await GetCurrentSession().LoadAsync<T>(Convert.ToInt64(identity)),
                nameof(Guid) => await GetCurrentSession().LoadAsync<T>(identity),
                _ => throw new InvalidOperationException()
            };
        }

        #endregion

        #region IEnumerable<T> Members

        public virtual IEnumerator<T> GetEnumerator() => GetCurrentSession().Query<T>().GetEnumerator();

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetCurrentSession().Query<T>().GetEnumerator();

        #endregion

        #region IQueryable Members

        public virtual Type ElementType => GetCurrentSession().Query<T>().ElementType;

        public virtual System.Linq.Expressions.Expression Expression => GetCurrentSession().Query<T>().Expression;

        public virtual IQueryProvider Provider => GetCurrentSession().Query<T>().Provider;

        #endregion
    }
}
