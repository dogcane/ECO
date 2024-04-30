namespace ECO.Data;

public sealed class NullDataTransaction(IPersistenceContext context) : IDataTransaction
{
    #region IDataTransaction Membri di

    public IPersistenceContext Context { get; private set; } = context ?? throw new ArgumentNullException(nameof(context));

    public void Commit() { }

    public void Rollback() { }

    #endregion

    #region IDisposable Membri di

    public void Dispose() { }

    public async Task CommitAsync(CancellationToken cancellationToken = default) => await Task.Run(Commit, cancellationToken);

    public async Task RollbackAsync(CancellationToken cancellationToken = default) => await Task.Run(Rollback, cancellationToken);

    #endregion
}
