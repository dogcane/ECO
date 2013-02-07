using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Raven.Client;
using Raven.Client.Document;

using ECO;
using ECO.Data;

namespace ECO.Providers.RavenDB
{
    public class RavenPersistenceContext : IPersistenceContext
    {
        #region Private_Fields

        private IDocumentSession _Session;

        #endregion

        #region Ctor

        public RavenPersistenceContext(IDocumentSession session)
        {
            _Session = session;
        }

        #endregion

        #region Public_Properties

        public IDocumentSession Session
        {
            get
            {
                return _Session;
            }
        }

        public IDataTransaction Transaction
        {
            get { throw new NotImplementedException(); }
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
