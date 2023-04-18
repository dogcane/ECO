using ECO.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace ECO.Providers.EntityFramework
{
    public class EntityFrameworkPersistenceManager<T, K> : PersistenceManagerBase<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Properties

        public DbContext DbContext => (PersistenceContext as EntityFrameworkPersistenceContext ?? throw new InvalidCastException(nameof(DbContext))).Context;

        #endregion

        #region Ctor

        public EntityFrameworkPersistenceManager(IDataContext dataContext) : base(dataContext)
        {
            
        }

        #endregion


    }
}
