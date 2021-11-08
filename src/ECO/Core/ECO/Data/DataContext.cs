using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace ECO.Data
{
    public sealed class DataContext : IDisposable, IDataContext
    {
        #region Private_Fields

        private readonly object _SyncLock = new object();

        private readonly IPersistenceUnitFactory _PersistenceUnitFactory;

        private readonly IDictionary<string, IPersistenceContext> _Contexts;

        private readonly ILogger<DataContext> _Logger;

        #endregion

        #region Public_Properties

        public Guid DataContextId { get; }

        public ITransactionContext Transaction { get; private set; }

        #endregion

        #region ~Ctor

        public DataContext(IPersistenceUnitFactory persistenceUnitFactory, ILoggerFactory loggerFactory)
        {
            DataContextId = Guid.NewGuid();
            _PersistenceUnitFactory = persistenceUnitFactory;
            _Contexts = new Dictionary<string, IPersistenceContext>();
            _Logger = loggerFactory.CreateLogger<DataContext>();
            _Logger.LogDebug("Data context is opening");
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
                        _Logger.LogDebug("Initialization of persistence context for entity {entityType}", entityType);
                        IPersistenceContext context = persistenceUnit.CreateContext();
                        _Contexts.Add(persistenceUnitName, context);
                        if (Transaction != null)
                        {
                            IDataTransaction tx = context.BeginTransaction();
                            Transaction.EnlistDataTransaction(tx);
                        }
                        _Logger.LogDebug("Persistence context for entity {entityType} already initialized => {persistenceUnitName}:{persistenceContextId}", entityType, persistenceUnitName, _Contexts[persistenceUnitName].PersistenceContextId);
                    }
                    else
                    {
                        _Logger.LogDebug("Persistence context for entity {entityType} initialized => {persistenceUnitName}:{persistenceContextId}", entityType, persistenceUnitName, _Contexts[persistenceUnitName].PersistenceContextId);
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
            if (Transaction == null)
            {
                lock (_SyncLock)
                {
                    if (Transaction == null)
                    {
                        Transaction = new TransactionContext(autoCommit);
                        _Logger.LogDebug("Starting a new transaction context : {transactionContextId}", Transaction.TransactionContextId);
                        foreach (IPersistenceContext persistenceContext in _Contexts.Values)
                        {
                            IDataTransaction tx = persistenceContext.BeginTransaction();
                            Transaction.EnlistDataTransaction(tx);
                        }
                    }
                }
            }
            return Transaction;
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
