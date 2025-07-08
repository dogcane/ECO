namespace ECO.Providers.InMemory;

using ECO.Data;

public class InMemoryReadOnlyRepository<T, K>(IDataContext dataContext)
    : InMemoryPersistenceManager<T, K>(dataContext), IReadOnlyRepository<T, K>
    where T : class, IAggregateRoot<K>
    where K : notnull
{
    #region IReadOnlyRepository<T,K> Membri di

    public virtual T? Load(K identity) => _EntitySet.TryGetValue(identity, out var entity) ? entity : null;

    public virtual ValueTask<T?> LoadAsync(K identity) => ValueTask.FromResult(Load(identity));

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
