using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO;
using ECO.Data;

namespace ECO.Providers.EntityFramework
{
    public class EntityFrameworkPersistenceManager<T, K> : PersistenceManagerBase<T, K> where T : IAggregateRoot<K>
    {
        #region Protected_Methods



        #endregion
    }
}
