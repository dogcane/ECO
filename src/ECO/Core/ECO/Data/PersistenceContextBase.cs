// Copyright (c) 2025 ECO Project. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;

namespace ECO.Data;

/// <summary>
/// Provides a base implementation for persistence contexts, managing the lifecycle and state of aggregates within a persistence unit.
/// Implements transaction management, entity tracking, and resource cleanup with support for logging and extensibility.
/// </summary>
/// <typeparam name="P">The concrete type of the persistence context, used for logging and type safety.</typeparam>
public abstract class PersistenceContextBase<P>(IPersistenceUnit persistenceUnit, ILogger<P>? logger = null) : IPersistenceContext
    where P : PersistenceContextBase<P>
{
    #region Protected_Fields
    /// <summary>
    /// Indicates whether the context has been disposed.
    /// </summary>
    protected bool _disposed = false;

    /// <summary>
    /// The logger instance for this context, if provided.
    /// </summary>
    protected readonly ILogger<P>? _Logger = logger;
    #endregion

    #region Ctor
    /// <summary>
    /// Finalizer to ensure unmanaged resources are released.
    /// </summary>
    ~PersistenceContextBase() => Dispose(false);
    #endregion

    #region Protected_Methods
    /// <summary>
    /// Begins a new transaction. Override to provide custom transaction logic.
    /// </summary>
    /// <returns>A new <see cref="IDataTransaction"/> instance.</returns>
    /// <remarks>
    /// Override this method in a derived class to return a custom transaction implementation.
    /// The default implementation returns a <see cref="NullDataTransaction"/>.
    /// </remarks>
    protected virtual IDataTransaction OnBeginTransaction() => new NullDataTransaction(this);

    /// <summary>
    /// Begins a new transaction asynchronously. Override to provide custom async transaction logic.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task that returns a new <see cref="IDataTransaction"/> instance.</returns>
    /// <remarks>
    /// Override this method in a derived class to provide asynchronous transaction creation logic.
    /// The default implementation wraps <see cref="OnBeginTransaction"/> in a completed task.
    /// </remarks>
    protected async virtual Task<IDataTransaction> OnBeginTransactionAsync(CancellationToken cancellationToken = default) => await Task.FromResult(OnBeginTransaction());

    /// <summary>
    /// Called when the context is being closed. Override to implement custom close logic.
    /// </summary>
    /// <remarks>
    /// Override this method to perform any cleanup or resource release when the context is closed.
    /// The default implementation does nothing.
    /// </remarks>
    protected virtual void OnClose() { }

    /// <summary>
    /// Called when the context is being disposed. Override to implement custom disposal logic.
    /// </summary>
    /// <remarks>
    /// Override this method to release any resources or perform cleanup during disposal.
    /// The default implementation does nothing.
    /// </remarks>
    protected virtual void OnDispose() { }

    /// <summary>
    /// Called to persist changes in the context. Override to implement custom save logic.
    /// </summary>
    /// <remarks>
    /// Override this method to implement how changes are persisted to the underlying store.
    /// The default implementation does nothing.
    /// </remarks>
    protected virtual void OnSaveChanges() { }

    /// <summary>
    /// Called to persist changes in the context asynchronously. Override to implement custom async save logic.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// Override this method to implement asynchronous persistence logic.
    /// The default implementation runs <see cref="OnSaveChanges"/> on a background thread.
    /// </remarks>
    protected virtual Task OnSaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnSaveChanges();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Called when an entity is attached to the context. Override to implement custom attach logic.
    /// </summary>
    /// <typeparam name="T">The type of the entity's identity.</typeparam>
    /// <param name="entity">The aggregate root to attach.</param>
    /// <remarks>
    /// Override this method to implement logic for tracking or initializing an entity when it is attached.
    /// The default implementation does nothing.
    /// </remarks>
    protected virtual void OnAttach<T>(IAggregateRoot<T> entity) { }

    /// <summary>
    /// Called when an entity is detached from the context. Override to implement custom detach logic.
    /// </summary>
    /// <typeparam name="T">The type of the entity's identity.</typeparam>
    /// <param name="entity">The aggregate root to detach.</param>
    /// <remarks>
    /// Override this method to implement logic for untracking or cleaning up an entity when it is detached.
    /// The default implementation does nothing.
    /// </remarks>
    protected virtual void OnDetach<T>(IAggregateRoot<T> entity) { }

    /// <summary>
    /// Called when an entity is refreshed in the context. Override to implement custom refresh logic.
    /// </summary>
    /// <typeparam name="T">The type of the entity's identity.</typeparam>
    /// <param name="entity">The aggregate root to refresh.</param>
    /// <remarks>
    /// Override this method to implement logic for reloading or updating an entity's state from the underlying store.
    /// The default implementation does nothing.
    /// </remarks>
    protected virtual void OnRefresh<T>(IAggregateRoot<T> entity) { }

    /// <summary>
    /// Gets the persistence state of an entity. Override to provide custom state logic.
    /// </summary>
    /// <typeparam name="T">The type of the entity's identity.</typeparam>
    /// <param name="entity">The aggregate root to check.</param>
    /// <returns>The persistence state of the entity.</returns>
    /// <remarks>
    /// Override this method to determine the persistence state (e.g., Added, Modified, Deleted) of an entity.
    /// The default implementation returns <see cref="PersistenceState.Unknown"/>.
    /// </remarks>
    protected virtual PersistenceState OnGetPersistenceState<T>(IAggregateRoot<T> entity) => PersistenceState.Unknown;
    #endregion

    #region IPersistenceContext_Members
    /// <inheritdoc />
    public Guid PersistenceContextId { get; } = Guid.NewGuid();

    /// <inheritdoc />
    public IPersistenceUnit PersistenceUnit { get; } = persistenceUnit ?? throw new ArgumentNullException(nameof(persistenceUnit));

    /// <inheritdoc />
    public IDataTransaction? Transaction { get; private set; }

    /// <inheritdoc />
    public void Attach<T>(IAggregateRoot<T> entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _Logger?.LogDebug("Attaching entity {entityName}:{entityId} in {name}:{id}", entity.GetType().Name, entity.Identity, PersistenceUnit.Name, PersistenceContextId);
        OnAttach(entity);
    }

    /// <inheritdoc />
    public void Detach<T>(IAggregateRoot<T> entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _Logger?.LogDebug("Detaching entity {entityName}:{entityId} in {name}:{id}", entity.GetType().Name, entity.Identity, PersistenceUnit.Name, PersistenceContextId);
        OnDetach(entity);
    }

    /// <inheritdoc />
    public void Refresh<T>(IAggregateRoot<T> entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _Logger?.LogDebug("Refreshing entity {entityName}:{entityId} in {name}:{id}", entity.GetType().Name, entity.Identity, PersistenceUnit.Name, PersistenceContextId);
        OnRefresh(entity);
    }

    /// <inheritdoc />
    public PersistenceState GetPersistenceState<T>(IAggregateRoot<T> entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _Logger?.LogDebug("Get Persistence State for entity {entityName}:{entityId} in {name}:{id}", entity.GetType().Name, entity.Identity, PersistenceUnit.Name, PersistenceContextId);
        return OnGetPersistenceState(entity);
    }

    /// <inheritdoc />
    public IDataTransaction BeginTransaction()
    {
        _Logger?.LogDebug("Begin transaction in {name}:{id}", PersistenceUnit.Name, PersistenceContextId);
        Transaction = OnBeginTransaction();
        return Transaction;
    }

    /// <inheritdoc />
    public async Task<IDataTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _Logger?.LogDebug("Begin transaction in {name}:{id}", PersistenceUnit.Name, PersistenceContextId);
        Transaction = await OnBeginTransactionAsync(cancellationToken);
        return Transaction;
    }

    /// <inheritdoc />
    public void Close()
    {
        OnClose();
        Dispose(true);
    }

    /// <inheritdoc />
    public void SaveChanges()
    {
        _Logger?.LogDebug("Save changes in {name}:{id}", PersistenceUnit.Name, PersistenceContextId);
        OnSaveChanges();
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        _Logger?.LogDebug("Save changes in {name}:{id}", PersistenceUnit.Name, PersistenceContextId);
        await OnSaveChangesAsync(cancellationToken);
    }
    #endregion

    #region IDisposable_Members
    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the context, releasing resources.
    /// </summary>
    /// <param name="isDisposing">True if called from Dispose; false if called from finalizer.</param>
    private void Dispose(bool isDisposing)
    {
        if (_disposed)
            return;

        if (isDisposing)
        {
            OnDispose();
        }
        _disposed = true;
    }
    #endregion
}
