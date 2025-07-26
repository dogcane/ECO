namespace ECO.Providers.EntityFramework;

using ECO.Data;
using Microsoft.EntityFrameworkCore;
using System;

/// <summary>
/// Entity Framework-specific persistence manager that provides access to the underlying DbContext.
/// </summary>
/// <typeparam name="T">The aggregate root type.</typeparam>
/// <typeparam name="K">The identity type of the aggregate root.</typeparam>
/// <param name="dataContext">The data context for this persistence manager.</param>
public class EntityFrameworkPersistenceManager<T, K>(IDataContext dataContext)
    : PersistenceManagerBase<T, K>(dataContext)
    where T : class, IAggregateRoot<K>
    where K : notnull, IEquatable<K>
{
    #region Properties
    
    /// <summary>
    /// Gets the Entity Framework DbContext from the persistence context.
    /// </summary>
    /// <exception cref="InvalidCastException">Thrown when the persistence context is not an EntityFrameworkPersistenceContext.</exception>
    public DbContext DbContext => 
        (PersistenceContext as EntityFrameworkPersistenceContext ?? 
         throw new InvalidCastException($"Expected {nameof(EntityFrameworkPersistenceContext)} but got {PersistenceContext?.GetType().Name}")).Context;
    
    #endregion
}
