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

        public virtual T? Load(K identity) => _EntitySet.TryGetValue(identity, out T entity) ? entity : null;

        public virtual async Task<T?> LoadAsync(K identity) => await Task.FromResult(Load(identity));

        #endregion

        #region IEnumerable<T> Membri di

        public virtual IEnumerator<T> GetEnumerator() => _EntitySet.Values.ToList().GetEnumerator();

        #endregion

        #region IEnumerable Membri di

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _EntitySet.Values.ToList().GetEnumerator();

        #endregion

        #region IQueryable Membri di

        public virtual Type ElementType => _EntitySet.AsQueryable().ElementType;

        public virtual System.Linq.Expressions.Expression Expression => _EntitySet.AsQueryable().Expression;

        public virtual IQueryProvider Provider => _EntitySet.AsQueryable().Provider;

        #endregion
    }
}
