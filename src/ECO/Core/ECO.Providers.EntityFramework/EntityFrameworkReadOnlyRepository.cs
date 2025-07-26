namespace ECO.Providers.EntityFramework;

using ECO.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

/// <summary>
/// Entity Framework implementation of a read-only repository for aggregate roots.
/// Provides query capabilities using EF Core DbContext.
/// </summary>
/// <typeparam name="T">The aggregate root type.</typeparam>
/// <typeparam name="K">The identity type of the aggregate root.</typeparam>
/// <param name="dataContext">The data context for this repository.</param>
public class EntityFrameworkReadOnlyRepository<T, K>(IDataContext dataContext)
    : EntityFrameworkPersistenceManager<T, K>(dataContext), IReadOnlyRepository<T, K>
    where T : class, IAggregateRoot<K>
    where K : notnull, IEquatable<K>
{
    #region IReadOnlyRepository<T,K> Members
    
    /// <inheritdoc />
    public virtual T? Load(K identity) => DbContext.Set<T>().Find(identity);
    
    /// <inheritdoc />
    public virtual ValueTask<T?> LoadAsync(K identity) => DbContext.Set<T>().FindAsync(identity);
    
    #endregion

    #region IEnumerable<T> Members
    
    /// <inheritdoc />
    public virtual IEnumerator<T> GetEnumerator() => DbContext.Set<T>().AsEnumerable().GetEnumerator();
    
    #endregion

    #region IEnumerable Members
    
    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    #endregion

    #region IQueryable Members
    
    /// <inheritdoc />
    public virtual Type ElementType => DbContext.Set<T>().AsQueryable().ElementType;
    
    /// <inheritdoc />
    public virtual Expression Expression => DbContext.Set<T>().AsQueryable().Expression;
    
    /// <inheritdoc />
    public virtual IQueryProvider Provider => DbContext.Set<T>().AsQueryable().Provider;
    
    #endregion
}
