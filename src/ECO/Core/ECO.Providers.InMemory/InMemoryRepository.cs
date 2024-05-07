using ECO.Data;

namespace ECO.Providers.InMemory;

public class InMemoryRepository<T, K>(IDataContext dataContext) : InMemoryReadOnlyRepository<T, K>(dataContext), IRepository<T, K>
    where T : class, IAggregateRoot<K>    
    where K : notnull
{
    #region IRepository<T> Membri di

    public virtual void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (item.Identity == null)
            throw new IdentityNotSetException();
        _EntitySet.TryAdd(item.Identity, item);
    }

    public virtual async Task AddAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        await Task.Run(() => Add(item));
    }

    public virtual void Update(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (item.Identity == null)
            throw new IdentityNotSetException();
        _EntitySet.TryUpdate(item.Identity, item, _EntitySet[item.Identity]);
    }

    public virtual async Task UpdateAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        await Task.Run(() => Update(item));
    }

    public virtual void Remove(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (item.Identity == null)
            throw new IdentityNotSetException();
        _EntitySet.TryRemove(item.Identity, out _);
    }

    public virtual async Task RemoveAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        await Task.Run(() => Remove(item));
    }

    #endregion
}
