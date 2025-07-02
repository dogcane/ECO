namespace ECO.Data;

/// <summary>
/// Represents a transaction context for managing multiple data transactions within a data context.
/// Provides methods for enlisting transactions, committing, and rolling back, both synchronously and asynchronously.
/// </summary>
public interface ITransactionContext : IDisposable, IAsyncDisposable
{
    #region Properties
    /// <summary>
    /// Gets the unique identifier for this transaction context instance.
    /// </summary>
    Guid TransactionContextId { get; }

    /// <summary>
    /// Gets a value indicating whether the transaction context is set to auto-commit.
    /// </summary>
    bool AutoCommit { get; }

    /// <summary>
    /// Gets the current status of the transaction context.
    /// </summary>
    TransactionStatus Status { get; }

    /// <summary>
    /// Gets the data context associated with this transaction context.
    /// </summary>
    IDataContext DataContext { get; }
    #endregion

    #region Methods
    /// <summary>
    /// Enlists a data transaction in this transaction context.
    /// </summary>
    /// <param name="dataTransaction">The data transaction to enlist.</param>
    void EnlistDataTransaction(IDataTransaction dataTransaction);

    /// <summary>
    /// Commits the transaction context.
    /// </summary>
    void Commit();

    /// <summary>
    /// Rolls back the transaction context.
    /// </summary>
    void Rollback();

    /// <summary>
    /// Asynchronously commits the transaction context.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous commit operation.</returns>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously rolls back the transaction context.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous rollback operation.</returns>
    Task RollbackAsync(CancellationToken cancellationToken = default);
    #endregion
}