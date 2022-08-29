﻿using ECO.Data;
using MongoDB.Driver;

namespace ECO.Providers.MongoDB
{
    public abstract class MongoPersistenceManager<T, K> : PersistenceManagerBase<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Ctor

        protected MongoPersistenceManager(IDataContext dataContext) : this(typeof(T).Name, dataContext)
        {

        }

        protected MongoPersistenceManager(string collectionName, IDataContext dataContext) : base(dataContext)
        {
            Database = (PersistenceContext as MongoPersistenceContext).Database;
            IdentityMap = (PersistenceContext as MongoPersistenceContext).IdentityMap;
            Collection = Database.GetCollection<T>(collectionName);
        }

        #endregion

        #region Properties

        public IMongoDatabase Database { get; }

        public MongoIdentityMap IdentityMap { get; }

        public IMongoCollection<T> Collection { get; }

        #endregion
    }
}
