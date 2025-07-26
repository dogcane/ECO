using ECO.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ECO.Providers.EntityFramework;

/// <summary>
/// Entity Framework implementation of the repository pattern for aggregate roots.
/// Provides CRUD operations using EF Core DbContext.
/// </summary>
/// <typeparam name="T">The aggregate root type.</typeparam>
/// <typeparam name="K">The identity type of the aggregate root.</typeparam>
/// <param name="dataContext">The data context for this repository.</param>
public class EntityFrameworkRepository<T, K>(IDataContext dataContext)
    : EntityFrameworkReadOnlyRepository<T, K>(dataContext), IRepository<T, K>
    where T : class, IAggregateRoot<K>
    where K : notnull, IEquatable<K>
{
    #region IRepository<T,K> Members
    
    /// <inheritdoc />
    public virtual void Add(T item) => 
        DbContext.Set<T>().Add(item ?? throw new ArgumentNullException(nameof(item)));

    /// <inheritdoc />
    public virtual async Task AddAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        await DbContext.Set<T>().AddAsync(item);
    }

    /// <inheritdoc />
    public virtual void Update(T item) => 
        DbContext.Set<T>().Update(item ?? throw new ArgumentNullException(nameof(item)));

    /// <inheritdoc />
    public virtual Task UpdateAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        Update(item);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public virtual void Remove(T item) => 
        DbContext.Set<T>().Remove(item ?? throw new ArgumentNullException(nameof(item)));

    /// <inheritdoc />
    public virtual Task RemoveAsync(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        Remove(item);
        return Task.CompletedTask;
    }
    
    #endregion
}
