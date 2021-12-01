using Microsoft.Extensions.Logging;
using System;

namespace ECO.Data
{
    public abstract class PersistenceContextBase<P> : IPersistenceContext
        where P : PersistenceContextBase<P>
    {
        #region Protected_Fields

        protected bool _disposed = false;

        protected readonly ILogger<P> _Logger;

        #endregion

        #region Ctor

        protected PersistenceContextBase(IPersistenceUnit persistenceUnit, ILogger<P> logger)
        {
            PersistenceContextId = Guid.NewGuid();
            PersistenceUnit = persistenceUnit;
            _Logger = logger;
        }

        ~PersistenceContextBase()
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

        protected virtual void OnAttach<T, K>(T entity) where T : IAggregateRoot<K>
        {

        }

        protected virtual void OnDetach<T, K>(T entity) where T : IAggregateRoot<K>
        {

        }

        protected virtual void OnRefresh<T, K>(T entity) where T : IAggregateRoot<K>
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
            _Logger.LogDebug("Detaching entity {entityName}:{entityId} in {persistenceUnitName}:{persistenceContextId}", entity.GetType().Name, entity.Identity, PersistenceUnit.Name, PersistenceContextId); ;
            OnDetach<T, K>(entity);
        }

        public void Refresh<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            _Logger.LogDebug("Refresh entity {entityName}:{entityId} in {persistenceUnitName}:{persistenceContextId}", entity.GetType().Name, entity.Identity, PersistenceUnit.Name, PersistenceContextId); ;
            OnRefresh<T, K>(entity);
        }

        public PersistenceState GetPersistenceState<T, K>(T entity)
            where T : IAggregateRoot<K>
        {
            return OnGetPersistenceState<T, K>(entity);
        }

        public IDataTransaction BeginTransaction()
        {
            _Logger.LogDebug("Begin transaction in {persistenceUnitName}:{persistenceContextId}", PersistenceUnit.Name, PersistenceContextId); ;
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
            _Logger.LogDebug("Save changes in {persistenceUnitName}:{persistenceContextId}", PersistenceUnit.Name, PersistenceContextId); ;
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
            if (_disposed)
                return;

            if (isDisposing)
            {
                OnDispose();
                GC.SuppressFinalize(this);
            }

            _disposed = true;
        }

        #endregion
    }
}
