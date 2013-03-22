using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECO;
using ECO.Data;

using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace ECO.Providers.MongoDB
{
    public class MongoPersistenceContext : IPersistenceContext
    {
        #region Properties

        public IDataTransaction Transaction
        {
            get { throw new NotSupportedException(); }
        }

        public MongoDatabase Database { get; protected set; }

        #endregion

        #region Ctor

        public MongoPersistenceContext(MongoDatabase database)
        {
            Database = database;
        }

        #endregion

        #region Methods

        public void Attach<T, K>(T entity) where T : IAggregateRoot<K>
        {
            
        }

        public void Detach<T, K>(T entity) where T : IAggregateRoot<K>
        {
            
        }

        public void Refresh<T, K>(T entity) where T : IAggregateRoot<K>
        {
            
        }

        public PersistenceState GetPersistenceState<T, K>(T entity) where T : IAggregateRoot<K>
        {
            throw new NotImplementedException();
        }

        public IDataTransaction BeginTransaction()
        {
            throw new NotSupportedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            
        }

        public void Dispose()
        {
            
        }

        #endregion
    }
}
