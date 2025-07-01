namespace ECO.Data;

/// <summary>
/// Represents a data context for managing persistence units, transactions, and aggregate root entities.
/// </summary>
public interface IDataContext : IDisposable
{
    #region Properties
    /// <summary>
    /// Gets the unique identifier for this data context instance.
    /// </summary>
    Guid DataContextId { get; }

    /// <summary>
    /// Gets the current transaction context, if any.
    /// </summary>
    ITransactionContext? Transaction { get; }
    #endregion

    #region Methods
    /// <summary>
    /// Closes the data context and releases resources.
    /// </summary>
    void Close();

    /// <summary>
    /// Saves all changes in all persistence contexts.
    /// </summary>
    void SaveChanges();

    /// <summary>
    /// Asynchronously saves all changes in all persistence contexts.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Attaches an aggregate root entity to the current context.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <param name="entity">The entity to attach.</param>
    void Attach<T>(IAggregateRoot<T> entity);

    /// <summary>
    /// Detaches an aggregate root entity from the current context.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <param name="entity">The entity to detach.</param>
    void Detach<T>(IAggregateRoot<T> entity);

    /// <summary>
    /// Refreshes an aggregate root entity in the current context.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <param name="entity">The entity to refresh.</param>
    void Refresh<T>(IAggregateRoot<T> entity);

    /// <summary>
    /// Begins a new transaction context.
    /// </summary>
    /// <returns>The transaction context.</returns>
    ITransactionContext BeginTransaction();

    /// <summary>
    /// Begins a new transaction context with optional auto-commit.
    /// </summary>
    /// <param name="autoCommit">Whether to auto-commit the transaction.</param>
    /// <returns>The transaction context.</returns>
    ITransactionContext BeginTransaction(bool autoCommit);

    /// <summary>
    /// Asynchronously begins a new transaction context.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The transaction context.</returns>
    Task<ITransactionContext> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously begins a new transaction context with optional auto-commit.
    /// </summary>
    /// <param name="autoCommit">Whether to auto-commit the transaction.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The transaction context.</returns>
    Task<ITransactionContext> BeginTransactionAsync(bool autoCommit, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current persistence context for the specified type.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <returns>The persistence context.</returns>
    /// <exception cref="PersistentClassNotRegisteredException">Thrown if the persistence context is not registered for the specified type.</exception>
    IPersistenceContext GetCurrentContext<T>();

    /// <summary>
    /// Gets the current persistence context for the specified entity instance.
    /// </summary>
    /// <param name="entity">The entity instance.</param>
    /// <returns>The persistence context.</returns>
    /// <exception cref="PersistentClassNotRegisteredException">Thrown if the persistence context is not registered for the specified entity type.</exception>
    IPersistenceContext GetCurrentContext(object entity);

    /// <summary>
    /// Gets the current persistence context for the specified entity type.
    /// </summary>
    /// <param name="entityType">The entity type.</param>
    /// <returns>The persistence context.</returns>
    /// <exception cref="PersistentClassNotRegisteredException">Thrown if the persistence context is not registered for the specified entity type.</exception>
    IPersistenceContext GetCurrentContext(Type entityType);

    /// <summary>
    /// Gets the persistence state of an aggregate root entity.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <param name="entity">The entity to check.</param>
    /// <returns>The persistence state.</returns>
    PersistenceState GetPersistenceState<T>(IAggregateRoot<T> entity);
    #endregion
}