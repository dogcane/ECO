namespace ECO.Providers.Marten;

using ECO.Data;
using System;
using System.Threading.Tasks;

public class MartenRepository<T,K>(IDataContext dataContext) : MartenReadOnlyRepository<T, K>(dataContext), IRepository<T, K>
    where T : class, IAggregateRoot<K>
{
    #region IRepository<T> Membri di

    public virtual void Add(T item) => DocumentSession.Store(item ?? throw new ArgumentNullException(nameof(item)));

    public virtual async Task AddAsync(T item) => await Task.Run(() => Add(item ?? throw new ArgumentNullException(nameof(item))));

    public virtual void Update(T item) => DocumentSession.Update(item ?? throw new ArgumentNullException(nameof(item)));

    public virtual async Task UpdateAsync(T item) => await Task.Run(() => DocumentSession.Update(item ?? throw new ArgumentNullException(nameof(item))));

    public virtual void Remove(T item) => DocumentSession.Delete(item ?? throw new ArgumentNullException(nameof(item)));

    public virtual async Task RemoveAsync(T item) => await Task.Run(() => DocumentSession.Delete(item ?? throw new ArgumentNullException(nameof(item))));

    #endregion
}
