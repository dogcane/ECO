using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECO;
using ECO.Data;
using MongoDB.Driver;

namespace ECO.Providers.MongoDB
{
    public class MongoPersistenceContext : IPersistenceContext
    {
        #region Fields

        private MongoDatabase _Database;

        #endregion

        #region Properties

        public IDataTransaction Transaction
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Ctor

        public MongoPersistenceContext(MongoDatabase database)
        {
            _Database = database;
        }

        #endregion

        #region Methods

        public void Attach<T, K>(T entity) where T : IAggregateRoot<K>
        {
            throw new NotImplementedException();
        }

        public void Detach<T, K>(T entity) where T : IAggregateRoot<K>
        {
            throw new NotImplementedException();
        }

        public void Refresh<T, K>(T entity) where T : IAggregateRoot<K>
        {
            throw new NotImplementedException();
        }

        public PersistenceState GetPersistenceState<T, K>(T entity) where T : IAggregateRoot<K>
        {
            throw new NotImplementedException();
        }

        public IDataTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
