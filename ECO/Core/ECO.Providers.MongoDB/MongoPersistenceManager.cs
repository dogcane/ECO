using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Data;

using MongoDB.Driver;

namespace ECO.Providers.MongoDB
{
    public class MongoPersistenceManager<T, K> : PersistenceManagerBase<T, K>
        where T : IAggregateRoot<K>
    {
        #region Protected_Methods

        protected MongoDatabase GetCurrentDatabase()
        {
            return (GetCurrentContext() as MongoPersistenceContext).Database;
        }

        #endregion
    }
}
