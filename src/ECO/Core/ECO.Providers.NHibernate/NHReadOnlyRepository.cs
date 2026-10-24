namespace ECO.Providers.NHibernate;

using ECO.Data;
using global::NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

public class NHReadOnlyRepository<T, K>(IDataContext dataContext) : NHPersistenceManager<T, K>(dataContext), IReadOnlyRepository<T, K>, IAsyncEnumerableSource<T>
    where T : class, IAggregateRoot<K>
    where K : notnull, IEquatable<K>
{
    #region IReadOnlyEntityManager<T,K> Members

    public virtual T? Load(K identity) => GetCurrentSession().Get<T>(identity);

    public virtual async ValueTask<T?> LoadAsync(K identity) => await GetCurrentSession().GetAsync<T>(identity);

    #endregion

    #region IAsyncEnumerableSource<T> Members

    /// <inheritdoc />
    public virtual async IAsyncEnumerable<T> ToAsyncEnumerable([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // NHibernate does not expose native asynchronous streaming: the query is materialized asynchronously.
        var entities = await GetCurrentSession().Query<T>().ToListAsync(cancellationToken).ConfigureAwait(false);
        foreach (var entity in entities)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return entity;
        }
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
