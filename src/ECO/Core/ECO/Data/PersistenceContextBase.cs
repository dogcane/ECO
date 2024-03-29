﻿using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Data
{
    public abstract class PersistenceContextBase<P> : IPersistenceContext
        where P : PersistenceContextBase<P>
    {
        #region Protected_Fields

        protected bool _disposed = false;

        protected readonly ILogger<P>? _Logger;

        #endregion

        #region Ctor

        protected PersistenceContextBase(IPersistenceUnit persistenceUnit, ILogger<P>? logger = null)
        {
            PersistenceContextId = Guid.NewGuid();
            PersistenceUnit = persistenceUnit ?? throw new ArgumentNullException(nameof(persistenceUnit));
            _Logger = logger;
        }

        ~PersistenceContextBase()
        {
            Dispose(false);
        }

        #endregion

        #region Protected_Methods

        protected virtual IDataTransaction OnBeginTransaction() => new NullDataTransaction(this);

        protected virtual async Task<IDataTransaction> OnBeginTransactionAsync(CancellationToken cancellationToken = default) => await Task.FromResult(OnBeginTransaction());

        protected virtual void OnClose()
        {

        }

        protected virtual void OnDispose()
        {

        }

        protected virtual void OnSaveChanges()
        {

        }
        protected virtual async Task OnSaveChangesAsync(CancellationToken cancellationToken = default) => await Task.Run(() => OnSaveChanges());

        protected virtual void OnAttach<T>(IAggregateRoot<T> entity)
        {

        }

        protected virtual void OnDetach<T>(IAggregateRoot<T> entity)
        {

        }

        protected virtual void OnRefresh<T>(IAggregateRoot<T> entity)
        {

        }

        protected virtual PersistenceState OnGetPersistenceState<T>(IAggregateRoot<T> entity) => PersistenceState.Unknown;

        #endregion

        #region IPersistenceContext Membri di

        public Guid PersistenceContextId { get; }

        public IPersistenceUnit PersistenceUnit { get; }

        public IDataTransaction? Transaction { get; private set; } = null;

        public void Attach<T>(IAggregateRoot<T> entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _Logger?.LogDebug($"Attaching entity {entity.GetType().Name}:{entity.Identity} in {PersistenceUnit.Name}:{PersistenceContextId}");
            OnAttach(entity);
        }

        public void Detach<T>(IAggregateRoot<T> entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _Logger?.LogDebug($"Detaching entity {entity.GetType().Name}:{entity.Identity} in {PersistenceUnit.Name}:{PersistenceContextId}");
            OnDetach(entity);
        }

        public void Refresh<T>(IAggregateRoot<T> entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _Logger?.LogDebug($"Refreshing entity {entity.GetType().Name}:{entity.Identity} in {PersistenceUnit.Name}:{PersistenceContextId}");
            OnRefresh(entity);
        }

        public PersistenceState GetPersistenceState<T>(IAggregateRoot<T> entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _Logger?.LogDebug($"Get Persistence State for entity {entity.GetType().Name}:{entity.Identity} in {PersistenceUnit.Name}:{PersistenceContextId}");
            return OnGetPersistenceState(entity);
        }

        public IDataTransaction BeginTransaction()
        {
            _Logger?.LogDebug($"Begin transaction in {PersistenceUnit.Name}:{PersistenceContextId}");
            Transaction = OnBeginTransaction();
            return Transaction;
        }

        public async Task<IDataTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _Logger?.LogDebug($"Begin transaction in {PersistenceUnit.Name}:{PersistenceContextId}");
            Transaction = await OnBeginTransactionAsync(cancellationToken);
            return Transaction;
        }

        public void Close()
        {
            OnClose();
            Dispose(true);
        }

        public void SaveChanges()
        {
            _Logger?.LogDebug($"Save changes in {PersistenceUnit.Name}:{PersistenceContextId}");
            OnSaveChanges();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _Logger?.LogDebug($"Save changes in {PersistenceUnit.Name}:{PersistenceContextId}");
            await OnSaveChangesAsync(cancellationToken);
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
