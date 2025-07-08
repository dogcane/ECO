using ECO.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ECO.Providers.EntityFramework;

public class EntityFrameworkRepository<T, K>(IDataContext dataContext)
    : EntityFrameworkReadOnlyRepository<T, K>(dataContext), IRepository<T, K>
    where T : class, IAggregateRoot<K>
{
    #region IRepository<T,K> Members
    public virtual void Add(T item) => DbContext.Set<T>().Add(item ?? throw new ArgumentNullException(nameof(item)));

    public virtual async Task AddAsync(T item)
    {
        await DbContext.Set<T>().AddAsync(item ?? throw new ArgumentNullException(nameof(item)));
    }

    public virtual void Update(T item) => DbContext.Set<T>().Update(item ?? throw new ArgumentNullException(nameof(item)));

    public virtual Task UpdateAsync(T item)
    {
        Update(item);
        return Task.CompletedTask;
    }

    public virtual void Remove(T item) => DbContext.Set<T>().Remove(item ?? throw new ArgumentNullException(nameof(item)));

    public virtual Task RemoveAsync(T item)
    {
        Remove(item);
        return Task.CompletedTask;
    }
    #endregion
}
