namespace ECO.Data;

/// <summary>
/// Represents a no-op (null object) implementation of <see cref="IDataTransaction"/>.
/// Used when a transaction is required by the API but no actual transaction is needed.
/// </summary>
public sealed class NullDataTransaction(IPersistenceContext context) : IDataTransaction
{
    /// <inheritdoc />
    public IPersistenceContext Context { get; } = context ?? throw new ArgumentNullException(nameof(context));

    /// <inheritdoc />
    public void Commit() { /* No operation */ }

    /// <inheritdoc />
    public void Rollback() { /* No operation */ }

    /// <inheritdoc />
    public void Dispose() { /* No resources to dispose */ }

    /// <inheritdoc />
    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        Commit();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        Rollback();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Asynchronously releases all resources used by the NullDataTransaction.
    /// Since this is a no-op transaction, no actual cleanup is performed.
    /// </summary>
    /// <returns>A completed ValueTask</returns>
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
