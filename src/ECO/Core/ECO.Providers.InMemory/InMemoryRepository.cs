namespace ECO.Providers.InMemory;

using ECO.Data;

public class InMemoryRepository<T, K>(IDataContext dataContext)
    : InMemoryReadOnlyRepository<T, K>(dataContext), IRepository<T, K>
    where T : class, IAggregateRoot<K>
    where K : notnull, IEquatable<K>
{
    #region IRepository<T> Membri di

    public virtual void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (item.Identity is null)
            throw new IdentityNotSetException();
        _EntitySet.TryAdd(item.Identity, item);
    }

    public virtual Task AddAsync(T item)
    {
        Add(item);
        return Task.CompletedTask;
    }

    public virtual void Update(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (item.Identity is null)
            throw new IdentityNotSetException();
        _EntitySet.TryUpdate(item.Identity, item, _EntitySet[item.Identity]);
    }

    public virtual Task UpdateAsync(T item)
    {
        Update(item);
        return Task.CompletedTask;
    }

    public virtual void Remove(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (item.Identity is null)
            throw new IdentityNotSetException();
        _EntitySet.TryRemove(item.Identity, out _);
    }

    public virtual Task RemoveAsync(T item)
    {
        Remove(item);
        return Task.CompletedTask;
    }

    #endregion
}
