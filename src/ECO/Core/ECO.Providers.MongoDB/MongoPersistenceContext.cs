using ECO.Data;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;

namespace ECO.Providers.MongoDB
{
    public class MongoPersistenceContext : PersistenceContextBase<MongoPersistenceContext>
    {
        #region Properties

        public IMongoDatabase Database { get; private set; }

        public MongoIdentityMap IdentityMap { get; private set; }

        #endregion

        #region Ctor

        public MongoPersistenceContext(IMongoDatabase database, IPersistenceUnit persistenceUnit, ILogger<MongoPersistenceContext>? logger)
            : base(persistenceUnit, logger)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            IdentityMap = new MongoIdentityMap();
        }

        #endregion
    }
}
