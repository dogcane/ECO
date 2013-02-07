using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO;
using ECO.Data;

namespace ECO.Providers.InMemory
{
    public class InMemoryPersistenceContext : IPersistenceContext
    {
        #region Fields

        protected static IDictionary<string, object> _InMemoryStorage = new Dictionary<string, object>();

        #endregion

        #region Properties

        public IDataTransaction Transaction
        {
            get { throw new NotImplementedException(); }
        }

        public object this[string indexer]
        {
            get
            {
                object storageValue;
                _InMemoryStorage.TryGetValue(indexer, out storageValue);
                return storageValue;
            }
            set
            {
                _InMemoryStorage[indexer] = value;
            }
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
            return PersistenceState.Unknown;
        }

        public IDataTransaction BeginTransaction()
        {
            return new InMemoryTransaction(this);   
        }

        public void Close()
        {
            
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
