using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECO.Data;
using ECO.Providers.MongoDB.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace ECO.Providers.MongoDB
{
    public abstract class MongoPersistenceManager<T, K> : PersistenceManagerBase<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Ctor

        protected MongoPersistenceManager()
        {
            
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

        protected MongoIdentityMap GetCurrentIdentityMap()
        {
            return (GetCurrentContext() as MongoPersistenceContext).IdentityMap;
        }

        #endregion
    }
}
