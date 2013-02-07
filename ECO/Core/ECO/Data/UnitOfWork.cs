using System;
using System.Collections.Generic;
using System.Text;

using ECO.Context;

namespace ECO.Data
{
    public sealed class UnitOfWork : IDisposable
    {
        #region Private_Fields

        private static readonly string UNITOFWORK_KEY = "UNITOFWORK_KEY";

        private DataContext _DataContext;

        private TransactionContext _TransactionContext;

        #endregion

        #region Public_Properties

        public static UnitOfWork Current
        {
            get
            {
                return ApplicationContext.GetContextData(UNITOFWORK_KEY) as UnitOfWork;
            }
        }

        #endregion

        #region ~Ctor

        public UnitOfWork()
            : this(false)
        {

        }

        public UnitOfWork(bool autoCommit)
        {
            ApplicationContext.SetContextData(UNITOFWORK_KEY, this);
            _DataContext = new DataContext();
            _TransactionContext = _DataContext.BeginTransaction(autoCommit);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        #endregion

        #region Public_Methods

        public void Attach<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            _DataContext.Attach<T, K>(entity);
        }

        public void Detach<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            _DataContext.Detach<T, K>(entity);
        }

        public PersistenceState GetPersistenceState<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            return _DataContext.GetPersistenceState<T, K>(entity);
        }

        public IPersistenceContext GetCurrentContext(object entity)
        {
            return _DataContext.GetCurrentContext(entity);
        }

        public void Commit()
        {
            _TransactionContext.Commit();
        }

        public void Rollback()
        {
            _TransactionContext.Rollback();
        }

        #endregion

        #region IDisposable Members

        public void Close()
        {
            Dispose(true);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _TransactionContext.Dispose();                
                _DataContext.Close();
                GC.SuppressFinalize(this);
            }
            ApplicationContext.SetContextData(UNITOFWORK_KEY, null);
            _TransactionContext = null;
            _DataContext = null;
        }

        #endregion
    }
}
