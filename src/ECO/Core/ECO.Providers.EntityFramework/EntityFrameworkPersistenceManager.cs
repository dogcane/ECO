namespace ECO.Providers.EntityFramework;

using ECO.Data;
using Microsoft.EntityFrameworkCore;
using System;

public class EntityFrameworkPersistenceManager<T, K>(IDataContext dataContext)
    : PersistenceManagerBase<T, K>(dataContext)
    where T : class, IAggregateRoot<K>
{
    #region Properties
    public DbContext DbContext => (PersistenceContext as EntityFrameworkPersistenceContext ?? throw new InvalidCastException(nameof(DbContext))).Context;
    #endregion
}
