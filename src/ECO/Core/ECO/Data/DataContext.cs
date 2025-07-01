using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace ECO.Data;

/// <summary>
/// Represents the main data context for managing persistence units and transactions.
/// </summary>
public sealed class DataContext : IDisposable, IDataContext
{
    #region Private_Fields

    private bool _disposed = false;
    private readonly IPersistenceUnitFactory _PersistenceUnitFactory;
    private readonly ILogger<DataContext>? _Logger;
    // Use thread-safe concurrent dictionary for contexts
    private readonly ConcurrentDictionary<string, IPersistenceContext> _Contexts = new();

    #endregion

    #region Public_Properties

    /// <summary>
    /// Gets the unique identifier for this data context instance.
    /// </summary>
    public Guid DataContextId { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets the current transaction context, if any.
    /// </summary>
    public ITransactionContext? Transaction { get; private set; } = null;

    #endregion

    #region ~Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="DataContext"/> class.
    /// </summary>
    /// <param name="persistenceUnitFactory">The persistence unit factory.</param>
    /// <param name="logger">The logger instance (optional).</param>
    public DataContext(IPersistenceUnitFactory persistenceUnitFactory, ILogger<DataContext>? logger = null)
    {
        _PersistenceUnitFactory = persistenceUnitFactory ?? throw new ArgumentNullException(nameof(persistenceUnitFactory));
        _Logger = logger;
        _Logger?.LogDebug("Data context '{DataContextId}' is opening", DataContextId);
    }

    /// <summary>
    /// Finalizer to ensure resources are released.
    /// </summary>
    ~DataContext()
    {
        Dispose(false);
    }

    #endregion

    #region Protected_Methods

    /// <summary>
    /// Initializes or returns an existing persistence context for the specified entity type.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    /// <returns>The persistence context for the entity type.</returns>
    private IPersistenceContext InitializeOrReturnContext(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);
        IPersistenceUnit persistenceUnit = _PersistenceUnitFactory.GetPersistenceUnit(entityType);
        string persistenceUnitName = persistenceUnit.Name;
        if (_Contexts.TryGetValue(persistenceUnitName, out IPersistenceContext? value))
        {
            _Logger?.LogDebug("Persistence context for entity {entityType} already initialized => '{persistenceUnitName}':'{persistenceContextId}'", entityType, persistenceUnitName, value.PersistenceContextId);
            return value;
        }
        else
        {
            _Logger?.LogDebug("Initialization of persistence context for entity '{entityType}'", entityType);
            IPersistenceContext context = persistenceUnit.CreateContext();
            _Contexts[persistenceUnitName] = context;
            Transaction?.EnlistDataTransaction(context.BeginTransaction());
            _Logger?.LogDebug("Persistence context for entity {entityType} initialized => '{persistenceUnitName}':'{persistenceContextId}'", entityType, persistenceUnitName, context.PersistenceContextId);
            return context;
        }
    }

    #endregion

    #region Public_Methods

    /// <summary>
    /// Begins a new transaction context.
    /// </summary>
    /// <returns>The transaction context.</returns>
    public ITransactionContext BeginTransaction() => BeginTransaction(false);

    /// <summary>
    /// Begins a new transaction context with optional auto-commit.
    /// </summary>
    /// <param name="autoCommit">Whether to auto-commit the transaction.</param>
    /// <returns>The transaction context.</returns>
    public ITransactionContext BeginTransaction(bool autoCommit)
    {
        if (Transaction is null or { Status: not TransactionStatus.Alive })
        {
            Transaction = new TransactionContext(this, autoCommit);
            _Logger?.LogDebug("Starting a new transaction context '{transactionContextId}'", Transaction.TransactionContextId);
            foreach (IPersistenceContext persistenceContext in _Contexts.Values)
            {
                IDataTransaction tx = persistenceContext.BeginTransaction();
                Transaction.EnlistDataTransaction(tx);
            }
            return Transaction;
        }
        else
        {
            throw new InvalidOperationException($"There is already an active transaction context with id '{Transaction?.TransactionContextId}'");
        }
    }

    /// <summary>
    /// Asynchronously begins a new transaction context.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The transaction context.</returns>
    public async Task<ITransactionContext> BeginTransactionAsync(CancellationToken cancellationToken = default) => await BeginTransactionAsync(false, cancellationToken);

    /// <summary>
    /// Asynchronously begins a new transaction context with optional auto-commit.
    /// </summary>
    /// <param name="autoCommit">Whether to auto-commit the transaction.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The transaction context.</returns>
    public async Task<ITransactionContext> BeginTransactionAsync(bool autoCommit, CancellationToken cancellationToken = default)
    {
        if (Transaction is null or { Status: not TransactionStatus.Alive })
        {
            Transaction = new TransactionContext(this, autoCommit);
            _Logger?.LogDebug("Starting a new transaction context '{transactionContextId}'", Transaction.TransactionContextId);
            foreach (IPersistenceContext persistenceContext in _Contexts.Values)
            {
                IDataTransaction tx = await persistenceContext.BeginTransactionAsync(cancellationToken);
                Transaction.EnlistDataTransaction(tx);
            }
            return Transaction;
        }
        else
        {
            throw new InvalidOperationException($"There is already an active transaction context with id '{Transaction?.TransactionContextId}'");
        }
    }

    /// <summary>
    /// Attaches an aggregate root entity to the current context.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <param name="entity">The entity to attach.</param>
    public void Attach<T>(IAggregateRoot<T> entity) => GetCurrentContext(entity).Attach(entity);

    /// <summary>
    /// Detaches an aggregate root entity from the current context.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <param name="entity">The entity to detach.</param>
    public void Detach<T>(IAggregateRoot<T> entity) => GetCurrentContext(entity).Detach(entity);

    /// <summary>
    /// Refreshes an aggregate root entity in the current context.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <param name="entity">The entity to refresh.</param>
    public void Refresh<T>(IAggregateRoot<T> entity) => GetCurrentContext(entity).Refresh(entity);

    /// <summary>
    /// Gets the persistence state of an aggregate root entity.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <param name="entity">The entity to check.</param>
    /// <returns>The persistence state.</returns>
    public PersistenceState GetPersistenceState<T>(IAggregateRoot<T> entity) => GetCurrentContext(entity).GetPersistenceState(entity);

    /// <summary>
    /// Gets the current persistence context for the specified type.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <returns>The persistence context.</returns>
    /// <exception cref="PersistentClassNotRegisteredException">Thrown if the persistence context is not registered for the specified type.</exception>
    public IPersistenceContext GetCurrentContext<T>() => GetCurrentContext(typeof(T));

    /// <summary>
    /// Gets the current persistence context for the specified entity instance.
    /// </summary>
    /// <param name="entity">The entity instance.</param>
    /// <returns>The persistence context.</returns>
    /// <exception cref="PersistentClassNotRegisteredException">Thrown if the persistence context is not registered for the specified entity type.</exception>
    public IPersistenceContext GetCurrentContext(object entity) => GetCurrentContext(entity.GetType());

    /// <summary>
    /// Gets the current persistence context for the specified entity type.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    /// <returns>The persistence context.</returns>
    /// <exception cref="PersistentClassNotRegisteredException">Thrown if the persistence context is not registered for the specified entity type.</exception>
    public IPersistenceContext GetCurrentContext(Type entityType) => InitializeOrReturnContext(entityType);

    /// <summary>
    /// Saves all changes in all persistence contexts.
    /// </summary>
    public void SaveChanges()
    {
        foreach (IPersistenceContext persistenceContext in _Contexts.Values)
        {
            persistenceContext.SaveChanges();
        }
    }

    /// <summary>
    /// Asynchronously saves all changes in all persistence contexts.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (IPersistenceContext persistenceContext in _Contexts.Values)
        {
            await persistenceContext.SaveChangesAsync(cancellationToken);
        }
    }

    #endregion

    #region IDisposable Members

    /// <summary>
    /// Closes the data context and releases resources.
    /// </summary>
    public void Close()
    {
        _Logger?.LogDebug("Data context '{DataContextId}' is closing", DataContextId);
        Dispose();
    }

    /// <summary>
    /// Disposes the data context and releases resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the data context, optionally releasing managed resources.
    /// </summary>
    /// <param name="isDisposing">Whether to dispose managed resources.</param>
    private void Dispose(bool isDisposing)
    {
        if (_disposed)
            return;

        if (isDisposing)
        {
            _Logger?.LogDebug("Data context '{DataContextId}' is disposing", DataContextId);
            Transaction?.Dispose();
            foreach (IPersistenceContext persistenceContext in _Contexts.Values)
            {
                persistenceContext.Dispose();
            }
            _disposed = true;
        }
    }

    #endregion
}
