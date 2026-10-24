namespace ECO.Providers.InMemory;

using ECO.Data;
using System.Runtime.CompilerServices;

public class InMemoryReadOnlyRepository<T, K>(IDataContext dataContext)
    : InMemoryPersistenceManager<T, K>(dataContext), IReadOnlyRepository<T, K>, IAsyncEnumerableSource<T>
    where T : class, IAggregateRoot<K>
    where K : notnull, IEquatable<K>
{
    #region IReadOnlyRepository<T,K> Membri di

    public virtual T? Load(K identity) => _EntitySet.TryGetValue(identity, out var entity) ? entity : null;

    public virtual ValueTask<T?> LoadAsync(K identity) => ValueTask.FromResult(Load(identity));

    #endregion

    #region IAsyncEnumerableSource<T> Members

    /// <inheritdoc />
    public virtual async IAsyncEnumerable<T> ToAsyncEnumerable([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // Enumerate a snapshot: entities added after the enumeration starts are not part of the current stream.
        foreach (var entity in _EntitySet.Values.ToList())
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return entity;
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

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
