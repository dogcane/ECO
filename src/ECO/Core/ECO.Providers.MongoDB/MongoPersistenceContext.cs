using ECO.Data;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ECO.Providers.MongoDB
{
    public class MongoPersistenceContext : PersistenceContextBase<MongoPersistenceContext>
    {
        #region Properties

        public IMongoDatabase Database { get; private set; }

        public MongoIdentityMap IdentityMap { get; private set; }

        #endregion

        #region Ctor

        public MongoPersistenceContext(IMongoDatabase database, IPersistenceUnit persistenceUnit, ILogger<MongoPersistenceContext> logger)
            : base(persistenceUnit, logger)
        {
            Database = database;
            IdentityMap = new MongoIdentityMap();
        }

        #endregion

        #region Methods

        /*
        protected override void OnRefresh<T>(IAggregateRoot<T> entity) => 
            entity = Database            
                .GetCollection<IAggregateRoot<T>>(entity.GetType().Name)
                .Find(Builders<IAggregateRoot<T>>.Filter.Eq(e => e.Identity, entity.Identity))
                .FirstOrDefault();
        */

        #endregion
    }
}
