namespace ECO.Providers.NHibernate;

using ECO.Data;
using System;
using System.Threading.Tasks;

public class NHRepository<T, K>(IDataContext dataContext) : NHReadOnlyRepository<T, K>(dataContext), IRepository<T, K>
    where T : class, IAggregateRoot<K>
    where K : notnull, IEquatable<K>
{
    #region IRepository<T> Membri di

    public virtual void Add(T item) => GetCurrentSession().Save(item);

    public virtual async Task AddAsync(T item) => await GetCurrentSession().SaveAsync(item);

    public virtual void Update(T item) => GetCurrentSession().Update(item); //Not necessary with auto-dirty-check

    public virtual async Task UpdateAsync(T item) => await GetCurrentSession().UpdateAsync(item);

    public virtual void Remove(T item) => GetCurrentSession().Delete(item);

    public virtual async Task RemoveAsync(T item) => await GetCurrentSession().DeleteAsync(item);

    #endregion
}
