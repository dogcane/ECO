using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ECO.Data
{
    public sealed class DataContext : IDisposable, IDataContext
    {
        #region Private_Fields

        private object _SyncLock = new object();

        private IPersistenceUnitFactory _PersistenceUnitFactory;

        private IDictionary<string, IPersistenceContext> _Contexts;

        private ITransactionContext _Transaction;

        #endregion

        #region Public_Properties

        public ITransactionContext Transaction => _Transaction;

        #endregion

        #region ~Ctor

        public DataContext(IPersistenceUnitFactory persistenceUnitFactory)
        {
            _PersistenceUnitFactory = persistenceUnitFactory;
            _Contexts = new Dictionary<string, IPersistenceContext>();
        }

        ~DataContext()
        {
            Dispose(false);
        }

        #endregion

        #region Protected_Methods

        private IPersistenceContext InitializeOrReturnContext(Type entityType)
        {
            IPersistenceUnit persistenceUnit = _PersistenceUnitFactory.GetPersistenceUnit(entityType);
            string persistenceUnitName = persistenceUnit.Name;                
            if (!_Contexts.ContainsKey(persistenceUnitName))
            {
                lock (_SyncLock)
                {
                    if (!_Contexts.ContainsKey(persistenceUnitName))
                    {
                        IPersistenceContext context = persistenceUnit.CreateContext();
                        _Contexts.Add(persistenceUnitName, context);
                        if (_Transaction != null)
                        {
                            IDataTransaction tx = context.BeginTransaction();
                            _Transaction.EnlistDataTransaction(tx);
                        }
                    }
                }
            }
            return _Contexts[persistenceUnitName];
        }

        #endregion

        #region Public_Methods

        public ITransactionContext BeginTransaction() => BeginTransaction(false);

        public ITransactionContext BeginTransaction(bool autoCommit)
        {
            if (_Transaction == null)
            {
                lock (_SyncLock)
                {
                    if (_Transaction == null)
                    {
                        _Transaction = new TransactionContext(autoCommit);
                        foreach (IPersistenceContext persistenceContext in _Contexts.Values)
                        {
                            IDataTransaction tx = persistenceContext.BeginTransaction();
                            _Transaction.EnlistDataTransaction(tx);
                        }
                    }
                }
            }
            return _Transaction;
        }        

        public void Attach<T, K>(T entity)
            where T : IAggregateRoot<K> => GetCurrentContext(entity).Attach<T, K>(entity);

        public void Detach<T, K>(T entity)
            where T : IAggregateRoot<K> => GetCurrentContext(entity).Detach<T, K>(entity);        

        public void Refresh<T, K>(T entity)
            where T : IAggregateRoot<K> => GetCurrentContext(entity).Refresh<T, K>(entity);

        public PersistenceState GetPersistenceState<T, K>(T entity)
            where T : IAggregateRoot<K> => GetCurrentContext(entity).GetPersistenceState<T, K>(entity);

        public IPersistenceContext GetCurrentContext<T>() => GetCurrentContext(typeof(T));

        public IPersistenceContext GetCurrentContext(object entity) => GetCurrentContext(entity.GetType());

        public IPersistenceContext GetCurrentContext(Type entityType) => InitializeOrReturnContext(entityType);

        public void SaveChanges()
        {
            lock (_SyncLock)
            {
                foreach (IPersistenceContext persistenceContext in _Contexts.Values)
                {
                    persistenceContext.SaveChanges();
                }
            }
            GC.SuppressFinalize(this);
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
                lock (_SyncLock)
                {
                    if (Transaction != null)
                    {
                        Transaction.Dispose();
                    }
                    foreach (IPersistenceContext persistenceContext in _Contexts.Values)
                    {
                        persistenceContext.Close();
                    }
                }
                GC.SuppressFinalize(this);
            }
        }

        #endregion
    }
}
