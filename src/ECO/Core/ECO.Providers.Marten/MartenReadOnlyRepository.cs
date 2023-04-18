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

        public virtual T? Load(K identity) => typeof(K).Name switch
        {
            nameof(String) => DocumentSession.Load<T>(Convert.ToString(identity)),
            nameof(Int32) => DocumentSession.Load<T>(Convert.ToInt32(identity)),
            nameof(Int64) => DocumentSession.Load<T>(Convert.ToInt64(identity)),
            nameof(Guid) => DocumentSession.Load<T>(Guid.Parse(Convert.ToString(identity))),
            _ => throw new InvalidOperationException()
        };


        public virtual async Task<T?> LoadAsync(K identity) => typeof(K).Name switch
        {
            nameof(String) => await DocumentSession.LoadAsync<T>(Convert.ToString(identity)),
            nameof(Int32) => await DocumentSession.LoadAsync<T>(Convert.ToInt32(identity)),
            nameof(Int64) => await DocumentSession.LoadAsync<T>(Convert.ToInt64(identity)),
            nameof(Guid) => await DocumentSession.LoadAsync<T>(Guid.Parse(Convert.ToString(identity))),
            _ => throw new InvalidOperationException()
        };

        #endregion

        #region IEnumerable<T> Members

        public virtual IEnumerator<T> GetEnumerator() => DocumentSession.Query<T>().GetEnumerator();

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => DocumentSession.Query<T>().GetEnumerator();

        #endregion

        #region IQueryable Members

        public virtual Type ElementType => DocumentSession.Query<T>().ElementType;

        public virtual System.Linq.Expressions.Expression Expression => DocumentSession.Query<T>().Expression;

        public virtual IQueryProvider Provider => DocumentSession.Query<T>().Provider;

        #endregion
    }
}
