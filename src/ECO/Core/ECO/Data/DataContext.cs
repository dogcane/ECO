using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ECO.Data
{
    public sealed class DataContext : IDisposable, IDataContext
    {
        #region Private_Fields

        private bool _disposed = false;

        private readonly object _SyncLock = new object();

        private readonly IPersistenceUnitFactory _PersistenceUnitFactory;        

        private readonly ILogger<DataContext> _Logger;

        private readonly IDictionary<string, IPersistenceContext> _Contexts = new Dictionary<string, IPersistenceContext>();

        #endregion

        #region Public_Properties

        public Guid DataContextId { get; } = Guid.NewGuid();

        public ITransactionContext Transaction { get; private set; }

        #endregion

        #region ~Ctor

        public DataContext(IPersistenceUnitFactory persistenceUnitFactory, ILogger<DataContext> logger = null)
        {
            _PersistenceUnitFactory = persistenceUnitFactory;
            _Logger = logger;
            _Logger?.LogDebug($"Data context '{DataContextId}' is opening");
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
                        _Logger?.LogDebug($"Initialization of persistence context for entity '{entityType}'");
                        IPersistenceContext context = persistenceUnit.CreateContext();
                        _Contexts.Add(persistenceUnitName, context);
                        if (Transaction != null)                        
                            Transaction.EnlistDataTransaction(context.BeginTransaction());                        
                        _Logger?.LogDebug($"Persistence context for entity {entityType} already initialized => '{persistenceUnitName}':'{_Contexts[persistenceUnitName].PersistenceContextId}'");
                    }
                    else
                    {
                        _Logger?.LogDebug($"Persistence context for entity {entityType} initialized => '{persistenceUnitName}':'{_Contexts[persistenceUnitName].PersistenceContextId}'");
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
            if (Transaction == null || Transaction?.Status != TransactionStatus.Alive)
            {
                lock (_SyncLock)
                {
                    if (Transaction == null || Transaction?.Status != TransactionStatus.Alive)
                    {
                        Transaction = new TransactionContext(autoCommit);
                        _Logger?.LogDebug($"Starting a new transaction context '{Transaction.TransactionContextId}'");
                        foreach (IPersistenceContext persistenceContext in _Contexts.Values)
                        {
                            IDataTransaction tx = persistenceContext.BeginTransaction();
                            Transaction.EnlistDataTransaction(tx);
                        }
                        return Transaction;
                    }
                }
            }
            throw new InvalidOperationException($"There is already an active transaction context with id '{Transaction?.TransactionContextId}'");
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
        }

        #endregion

        #region IDisposable Members

        public void Close()
        {
            _Logger?.LogDebug($"Data context '{DataContextId}' is closing");
            Dispose(true);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (_disposed)
                return;

            if (isDisposing)
            {
                _Logger?.LogDebug($"Data context '{DataContextId}' is disposing");

                lock (_SyncLock)
                {
                    if (Transaction != null)
                    {
                        Transaction.Dispose();
                    }
                    foreach (IPersistenceContext persistenceContext in _Contexts.Values)
                    {
                        persistenceContext.Dispose();
                    }
                }
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        #endregion
    }
}
