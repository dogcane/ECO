namespace ECO.Data;

/// <summary>
/// Represents a data transaction for a persistence context.
/// Provides synchronous and asynchronous methods for commit and rollback operations.
/// </summary>
public interface IDataTransaction : IDisposable, IAsyncDisposable
{
    #region Properties
    /// <summary>
    /// Gets the persistence context that owns this data transaction.
    /// </summary>
    IPersistenceContext Context { get; }
    #endregion

    #region Methods
    /// <summary>
    /// Commits the transaction.
    /// </summary>
    void Commit();

    /// <summary>
    /// Rolls back the transaction.
    /// </summary>
    void Rollback();

    /// <summary>
    /// Asynchronously commits the transaction.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous commit operation.</returns>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously rolls back the transaction.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous rollback operation.</returns>
    Task RollbackAsync(CancellationToken cancellationToken = default);
    #endregion
}
