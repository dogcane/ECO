using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECO;
using ECO.Data;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace ECO.Providers.MongoDB
{
    public class MongoPersistenceContext : IPersistenceContext
    {
        #region Properties

        public IDataTransaction Transaction { get; protected set; }

        public MongoDatabase Database { get; private set; }

        public MongoIdentityMap IdentityMap { get; private set; }

        #endregion

        #region Ctor

        public MongoPersistenceContext(MongoDatabase database)
        {
            Database = database;
            IdentityMap = new MongoIdentityMap();
        }

        #endregion

        #region Methods

        public void Attach<T, K>(T entity) where T : IAggregateRoot<K>
        {
            //Not needed
        }

        public void Detach<T, K>(T entity) where T : IAggregateRoot<K>
        {
            //Not needed
        }

        public void Refresh<T, K>(T entity) where T : IAggregateRoot<K>
        {
            entity = Database.GetCollection<T>(typeof(T).Name).FindOneAs<T>(Query<T>.EQ(e => e.Identity, entity.Identity));
        }

        public PersistenceState GetPersistenceState<T, K>(T entity) where T : IAggregateRoot<K>
        {
            return PersistenceState.Unknown;
        }

        public IDataTransaction BeginTransaction()
        {
            Transaction = new NullDataTransaction(this);
            return Transaction;
        }

        public IDataTransaction BeginTransaction(IsolationLevel isolationLevel) //isolationLevel not supported by memory transaction
        {
            return BeginTransaction();
        }

        public void Close()
        {
            //Not needed
        }

        public void SaveChanges()
        {
            //Not needed
        }

        public void Dispose()
        {
            //Not needed
        }

        #endregion
    }
}
