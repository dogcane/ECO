namespace ECO.Data;

/// <summary>
/// Represents a transaction context that manages the lifecycle and coordination of multiple data transactions.
/// </summary>
public sealed class TransactionContext : ITransactionContext
{
    #region Private_Fields

    private bool _disposed = false;
    private readonly List<IDataTransaction> _Transactions = [];

    #endregion

    #region Public_Properties

    /// <inheritdoc/>
    public Guid TransactionContextId { get; } = Guid.NewGuid();

    /// <inheritdoc/>
    public TransactionStatus Status { get; private set; } = TransactionStatus.Alive;

    /// <inheritdoc/>
    public bool AutoCommit { get; }

    /// <inheritdoc/>
    public IDataContext DataContext { get; private set; }

    #endregion

    #region ~Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionContext"/> class.
    /// </summary>
    /// <param name="dataContext">The data context associated with this transaction context.</param>
    internal TransactionContext(IDataContext dataContext)
        : this(dataContext, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionContext"/> class with optional auto-commit.
    /// </summary>
    /// <param name="dataContext">The data context associated with this transaction context.</param>
    /// <param name="autoCommit">Whether to automatically commit on dispose.</param>
    internal TransactionContext(IDataContext dataContext, bool autoCommit)
    {
        DataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        AutoCommit = autoCommit;
    }

    /// <summary>
    /// Finalizer to ensure resources are released.
    /// </summary>
    ~TransactionContext() => Dispose(false);

    #endregion

    #region Public_Methods

    /// <inheritdoc/>
    public void EnlistDataTransaction(IDataTransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        _Transactions.Add(transaction);
    }

    /// <inheritdoc/>
    public void Commit()
    {
        foreach (var tx in _Transactions)
        {
            tx.Commit();
        }
        Status = TransactionStatus.Committed;
    }

    /// <inheritdoc/>
    public void Rollback()
    {
        foreach (var tx in _Transactions)
        {
            tx.Rollback();
        }
        Status = TransactionStatus.RolledBack;
    }

    /// <inheritdoc/>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        foreach (var tx in _Transactions)
        {
            await tx.CommitAsync(cancellationToken).ConfigureAwait(false);
        }
        Status = TransactionStatus.Committed;
    }

    /// <inheritdoc/>
    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        foreach (var tx in _Transactions)
        {
            await tx.RollbackAsync(cancellationToken).ConfigureAwait(false);
        }
        Status = TransactionStatus.RolledBack;
    }

    #endregion

    #region IDisposable Members

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool isDisposing)
    {
        if (_disposed)
            return;

        if (isDisposing)
        {
            if (Status is TransactionStatus.Alive && AutoCommit)
            {
                Commit();
                Status = TransactionStatus.Committed;
            }
            foreach (var tx in _Transactions)
            {
                tx.Dispose();
            }
        }
        _disposed = true;
    }

    #endregion

    #region IAsyncDisposable Members

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore(true).ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    private async ValueTask DisposeAsync(bool isDisposing)
    {
        if (_disposed)
            return;

        if (isDisposing)
        {
            if (Status is TransactionStatus.Alive && AutoCommit)
            {
                await CommitAsync().ConfigureAwait(false);
                Status = TransactionStatus.Committed;
            }
            foreach (var tx in _Transactions)
            {
                await tx.DisposeAsync().ConfigureAwait(false);
            }
        }
        _disposed = true;
    }

    #endregion
}
