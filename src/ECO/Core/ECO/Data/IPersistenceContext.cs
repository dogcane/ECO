namespace ECO.Data;

/// <summary>
/// Represents a persistence context for managing aggregate root entities and transactions.
/// Provides methods for attaching, detaching, refreshing entities, and managing transactions and persistence state.
/// </summary>
public interface IPersistenceContext : IDisposable
{
    #region Properties
    /// <summary>
    /// Gets the unique identifier for this persistence context instance.
    /// </summary>
    Guid PersistenceContextId { get; }

    /// <summary>
    /// Gets the persistence unit associated with this context.
    /// </summary>
    IPersistenceUnit PersistenceUnit { get; }

    /// <summary>
    /// Gets the current data transaction, if any.
    /// </summary>
    IDataTransaction? Transaction { get; }
    #endregion

    #region Methods
    /// <summary>
    /// Attaches an aggregate root entity to the context.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <param name="entity">The entity to attach.</param>
    void Attach<T>(IAggregateRoot<T> entity);

    /// <summary>
    /// Detaches an aggregate root entity from the context.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <param name="entity">The entity to detach.</param>
    void Detach<T>(IAggregateRoot<T> entity);

    /// <summary>
    /// Refreshes an aggregate root entity in the context.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <param name="entity">The entity to refresh.</param>
    void Refresh<T>(IAggregateRoot<T> entity);

    /// <summary>
    /// Gets the persistence state of an aggregate root entity.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <param name="entity">The entity to check.</param>
    /// <returns>The persistence state.</returns>
    PersistenceState GetPersistenceState<T>(IAggregateRoot<T> entity);

    /// <summary>
    /// Begins a new data transaction in the context.
    /// </summary>
    /// <returns>The data transaction.</returns>
    IDataTransaction BeginTransaction();

    /// <summary>
    /// Asynchronously begins a new data transaction in the context.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation, with the data transaction.</returns>
    Task<IDataTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Closes the persistence context and releases resources.
    /// </summary>
    void Close();

    /// <summary>
    /// Saves all changes in the context.
    /// </summary>
    void SaveChanges();

    /// <summary>
    /// Asynchronously saves all changes in the context.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous save operation.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    #endregion
}
