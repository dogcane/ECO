namespace ECO.Providers.Marten;

using ECO.Data;
using System;
using System.Threading.Tasks;

public class MartenRepository<T,K>(IDataContext dataContext) : MartenReadOnlyRepository<T, K>(dataContext), IRepository<T, K>
    where T : class, IAggregateRoot<K>
    where K : notnull, IEquatable<K>
{
    #region IRepository<T> Membri di

    public virtual void Add(T item) => DocumentSession.Store(item ?? throw new ArgumentNullException(nameof(item)));

    public virtual Task AddAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        DocumentSession.Store(item);
        return Task.CompletedTask;
    }

    public virtual void Update(T item) => DocumentSession.Update(item ?? throw new ArgumentNullException(nameof(item)));

    public virtual Task UpdateAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        DocumentSession.Update(item);
        return Task.CompletedTask;
    }

    public virtual void Remove(T item) => DocumentSession.Delete(item ?? throw new ArgumentNullException(nameof(item)));

    public virtual Task RemoveAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        DocumentSession.Delete(item);
        return Task.CompletedTask;
    }

    #endregion
}
