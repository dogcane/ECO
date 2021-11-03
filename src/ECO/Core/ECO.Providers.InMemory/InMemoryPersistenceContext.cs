using ECO.Data;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ECO.Providers.InMemory
{
    public class InMemoryPersistenceContext : IPersistenceContext
    {
        #region Properties

        public IDataTransaction Transaction { get; protected set; }

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
            Transaction = new NullDataTransaction(this);
            return Transaction;
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
