using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Data;

using MongoDB.Driver;

namespace ECO.Providers.MongoDB
{
    public abstract class MongoPersistenceManager<T, K> : PersistenceManagerBase<T, K>
        where T : IAggregateRoot<K>
    {
        #region Ctor

        protected MongoPersistenceManager()
        {
            ConfigureMapping();
        }

        #endregion

        #region Protected_Methods

        protected MongoDatabase GetCurrentDatabase()
        {
            return (GetCurrentContext() as MongoPersistenceContext).Database;
        }

        protected MongoCollection GetCurrentCollection()
        {
            return GetCurrentDatabase().SafeGetCollectionForType<T>();
        }

        protected abstract void ConfigureMapping();

        #endregion
    }
}
