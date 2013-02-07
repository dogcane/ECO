using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECO.Data
{
    public abstract class PersistentContextBase : IPersistenceContext
    {
        #region Protected_Methods

        protected virtual void OnAttach<T, K>(T entity)
            where T : IAggregateRoot<K>
        {

        }

        #endregion

        #region IPersistenceContext Membri di

        public abstract IDataTransaction Transaction
        {
            get;
        }

        public void Attach<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            OnAttach<T, K>(entity);
        }

        public void Detach<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            throw new NotImplementedException();
        }

        public void Refresh<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            throw new NotImplementedException();
        }

        public PersistenceState GetPersistenceState<T, K>(T entity)
            where T : IAggregateRoot<K>
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

        #endregion

        #region IDisposable Membri di

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
