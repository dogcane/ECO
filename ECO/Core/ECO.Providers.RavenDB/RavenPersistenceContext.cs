using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Raven.Client;
using Raven.Client.Document;

using ECO;
using ECO.Data;
using System.Data;

namespace ECO.Providers.RavenDB
{
    public class RavenPersistenceContext : IPersistenceContext
    {
        #region Ctor

        public RavenPersistenceContext(IAsyncDocumentSession session)
        {
            Session = session;
        }

        #endregion

        #region Public_Properties

        public IAsyncDocumentSession Session { get; protected set; }

        public IDataTransaction Transaction { get; protected set; }

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
            Transaction = new NullDataTransaction(this);
            return Transaction;
        }

        public IDataTransaction BeginTransaction(IsolationLevel isolationLevel) //isolationLevel not supported by memory transaction
        {
            return BeginTransaction();
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
