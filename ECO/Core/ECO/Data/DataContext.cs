using System;
using System.Collections.Generic;
using System.Text;

using ECO.Context;

namespace ECO.Data
{
    public sealed class DataContext : IDisposable
    {
        #region Private_Fields

        private static readonly string DATACONTEXT_KEY = "DATACONTEXT_KEY";

        private Dictionary<string, IPersistenceContext> _Contexts;

        private TransactionContext _Transaction;

        #endregion

        #region Public_Properties

        public static DataContext Current
        {
            get
            {
                return ApplicationContext.GetContextData(DATACONTEXT_KEY) as DataContext;
            }
        }

        public TransactionContext Transaction
        {
            get
            {
                return _Transaction;
            }
        }

        public IPersistenceContext this[string name]
        {
            get
            {
                return _Contexts[name];
            }
        }

        #endregion

        #region ~Ctor

        public DataContext()
        {
            ApplicationContext.SetContextData(DATACONTEXT_KEY, this);
            _Contexts = new Dictionary<string, IPersistenceContext>();
        }

        ~DataContext()
        {
            Dispose(false);
        }

        #endregion

        #region Public_Methods

        public TransactionContext BeginTransaction()
        {
            return BeginTransaction(false);
        }

        public TransactionContext BeginTransaction(bool autoCommit)
        {
            _Transaction = new TransactionContext(autoCommit);
            foreach (string contextKey in _Contexts.Keys)
            {
                IPersistenceContext context = _Contexts[contextKey];
                IDataTransaction tx = context.BeginTransaction();
                _Transaction.AddDataTransaction(tx);
            }
            return _Transaction;
        }

        public bool IsContextInitialized(string persistenceUnitName)
        {
            return _Contexts.ContainsKey(persistenceUnitName);
        }

        public void InitializeContext(string persistenceUnitName, IPersistenceContext context)
        {
            if (!IsContextInitialized(persistenceUnitName))
            {
                _Contexts.Add(persistenceUnitName, context);
                if (_Transaction != null)
                {
                    IDataTransaction tx = context.BeginTransaction();
                    _Transaction.AddDataTransaction(tx);
                }
            }
        }

        public void Attach<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            IPersistenceContext RealContext = PersistenceUnitFactory.Instance.GetPersistenceUnit(entity.GetType()).GetCurrentContext();
            RealContext.Attach<T, K>(entity);
        }

        public void Detach<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            IPersistenceContext RealContext = PersistenceUnitFactory.Instance.GetPersistenceUnit(entity.GetType()).GetCurrentContext();
            RealContext.Detach<T, K>(entity);
        }

        public void Refresh<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            IPersistenceContext RealContext = PersistenceUnitFactory.Instance.GetPersistenceUnit(entity.GetType()).GetCurrentContext();
            RealContext.Refresh<T, K>(entity);
        }

        public PersistenceState GetPersistenceState<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            IPersistenceContext RealContext = PersistenceUnitFactory.Instance.GetPersistenceUnit(entity.GetType()).GetCurrentContext();
            return RealContext.GetPersistenceState<T, K>(entity);
        }

        public IPersistenceContext GetCurrentContext(object entity)
        {
            return GetCurrentContext(entity.GetType());
        }

        public IPersistenceContext GetCurrentContext(Type entityType)
        {
            IPersistenceContext RealContext = PersistenceUnitFactory.Instance.GetPersistenceUnit(entityType).GetCurrentContext();
            return RealContext;
        }

        public void SaveChanges()
        {
            foreach (string persistenceUnitName in _Contexts.Keys)
            {
                _Contexts[persistenceUnitName].SaveChanges();
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
                if (Transaction != null)
                {
                    Transaction.Dispose();
                }
                foreach (string persistenceUnitName in _Contexts.Keys)
                {
                    _Contexts[persistenceUnitName].Close();
                }
                GC.SuppressFinalize(this);
            }
            ApplicationContext.SetContextData(DATACONTEXT_KEY, null);
        }

        #endregion
    }
}
