namespace ECO.Providers.MongoDB;

using System;
using ECO.Data;
using global::MongoDB.Driver;
using Microsoft.Extensions.Logging;

public class MongoPersistenceContext(IMongoDatabase database, IPersistenceUnit persistenceUnit, ILogger<MongoPersistenceContext>? logger) : PersistenceContextBase<MongoPersistenceContext>(persistenceUnit, logger)
{
    #region Properties
    public IMongoDatabase Database { get; } = database ?? throw new ArgumentNullException(nameof(database));
    public MongoIdentityMap IdentityMap { get; } = new MongoIdentityMap();
    #endregion
}
