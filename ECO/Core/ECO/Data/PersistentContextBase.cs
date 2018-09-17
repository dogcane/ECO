using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ECO.Data
{
    public abstract class PersistentContextBase : IPersistenceContext
    {
        #region Protected_Methods

        protected abstract IDataTransaction OnBeginTransaction(IsolationLevel? transactionLevel);

        protected virtual void OnDispose()
        {

        }

        #endregion

        #region IPersistenceContext Membri di

        public virtual IDataTransaction Transaction { get; protected set; }

        public virtual void Attach<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            
        }

        public virtual void Detach<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            
        }

        public virtual void Refresh<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            
        }

        public virtual PersistenceState GetPersistenceState<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            return PersistenceState.Unknown;
        }

        public IDataTransaction BeginTransaction()
        {
            Transaction = OnBeginTransaction(null);
            return Transaction;
        }

        public IDataTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            Transaction = OnBeginTransaction(isolationLevel);
            return Transaction;
        }

        public virtual void Close()
        {
            Dispose(true);
        }

        public virtual void SaveChanges()
        {
            
        }

        #endregion

        #region IDisposable Membri di

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                OnDispose();
                GC.SuppressFinalize(this);
            }
        }

        ~PersistentContextBase()
        {
            Dispose(false);
        }

        #endregion
    }
}
