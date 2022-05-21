using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO;
using ECO.Data;

namespace ECO.Providers.EntityFramework
{
    public class EntityFrameworkPersistenceManager<T, K> : PersistenceManagerBase<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Protected_Methods

        protected DbContext GetCurrentDbContext()
        {
            return (GetCurrentContext() as EntityFrameworkPersistenceContext).Context;
        }

        protected DbContextTransaction GetCurrentTransaction()
        {
            return (GetCurrentContext().Transaction as EntityFrameworkDataTransaction).Transaction;
        }

        #endregion
    }
}
