using System;
using Microsoft.Extensions.Logging;

namespace ECO.Data
{
    public abstract class PersistentContextBase<P> : IPersistenceContext
        where P : PersistentContextBase<P>
    {
        #region Protected_Fields

        protected readonly ILogger<P> _Logger;

        #endregion

        #region Ctor

        protected PersistentContextBase(IPersistenceUnit persistenceUnit, ILoggerFactory loggerFactory)
        {
            PersistenceContextId = Guid.NewGuid();
            PersistenceUnit = persistenceUnit;
            _Logger = loggerFactory.CreateLogger<P>();
        }

        ~PersistentContextBase()
        {
            Dispose(false);
        }

        #endregion

        #region Protected_Methods

        protected abstract IDataTransaction OnBeginTransaction();

        protected virtual void OnClose()
        {
            
        }

        protected virtual void OnDispose()
        {

        }

        protected virtual void OnSaveChanges()
        {

        }

        protected virtual void OnAttach<T, K>(T entity)
        {

        }

        protected virtual void OnDetach<T, K>(T entity)
        {

        }

        protected virtual void OnRefresh<T, K>(T entity)
        {

        }

        protected virtual PersistenceState OnGetPersistenceState<T, K>(T entity)
        {
            return PersistenceState.Unknown;
        }

        #endregion

        #region IPersistenceContext Membri di

        public Guid PersistenceContextId { get; }

        public IPersistenceUnit PersistenceUnit { get; }

        public IDataTransaction Transaction { get; private set; }  

        public void Attach<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            _Logger.LogDebug("Attaching entity {entityName}:{entityId} in {persistenceUnitName}:{persistenceContextId}", entity.GetType().Name, entity.Identity, PersistenceUnit.Name, PersistenceContextId); ;
            OnAttach<T, K>(entity);
        }

        public void Detach<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            OnDetach<T, K>(entity);
        }

        public void Refresh<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            OnRefresh<T, K>(entity);
        }

        public PersistenceState GetPersistenceState<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            return OnGetPersistenceState<T, K>(entity);
        }

        public IDataTransaction BeginTransaction()
        {
            Transaction = OnBeginTransaction();
            return Transaction;
        }

        public void Close()
        {
            OnClose();
            Dispose(true);
        }

        public void SaveChanges()
        {
            OnSaveChanges();
        }

        #endregion

        #region IDisposable Membri di

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                OnDispose();
                GC.SuppressFinalize(this);
            }
        }

        #endregion
    }
}
