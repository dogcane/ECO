namespace ECO.Providers.MongoDB;

using ECO.Data;
using Mongo=global::MongoDB.Driver;
using System;

public abstract class MongoPersistenceManager<T, K> : PersistenceManagerBase<T, K>
    where T : class, IAggregateRoot<K>
{
    #region Ctor
    protected MongoPersistenceManager(IDataContext dataContext) : this(typeof(T).Name, dataContext) { }

    protected MongoPersistenceManager(string collectionName, IDataContext dataContext) : base(dataContext)
    {
        if (PersistenceContext is not MongoPersistenceContext mongoContext)
            throw new InvalidCastException($"PersistenceContext must be MongoPersistenceContext for collection '{collectionName}'");
        Database = mongoContext.Database;
        IdentityMap = mongoContext.IdentityMap;
        Collection = Database.GetCollection<T>(collectionName);
    }
    #endregion

    #region Properties
    public Mongo.IMongoDatabase Database { get; }
    public MongoIdentityMap IdentityMap { get; }
    public Mongo.IMongoCollection<T> Collection { get; }
    #endregion
}
