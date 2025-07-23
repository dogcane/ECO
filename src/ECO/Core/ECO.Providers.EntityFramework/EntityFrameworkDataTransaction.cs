namespace ECO.Providers.EntityFramework;

using ECO.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Entity Framework-specific data transaction that wraps an EF Core database transaction.
/// </summary>
public sealed class EntityFrameworkDataTransaction : IDataTransaction, IAsyncDisposable
{
    #region Fields    
    private bool _disposed = false;    
    #endregion

    #region IDataTransaction Members    
    /// <summary>
    /// Gets the underlying Entity Framework database transaction.
    /// </summary>
    public IDbContextTransaction Transaction { get; private init; }    
    /// <summary>
    /// Gets the Entity Framework persistence context associated with this transaction.
    /// </summary>
    public EntityFrameworkPersistenceContext Context { get; private init; }    
    /// <inheritdoc />
    IPersistenceContext IDataTransaction.Context => Context;    
    #endregion

    #region Constructor
    
    /// <summary>
    /// Initializes a new instance of the EntityFrameworkDataTransaction class.
    /// </summary>
    /// <param name="context">The persistence context that owns this transaction.</param>
    /// <param name="transaction">The underlying EF Core database transaction.</param>
    private EntityFrameworkDataTransaction(EntityFrameworkPersistenceContext context, IDbContextTransaction transaction)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
    }
    
    /// <summary>
    /// Finalizer to ensure resources are released if disposal is missed.
    /// </summary>
    ~EntityFrameworkDataTransaction() => Dispose(false);
    
    #endregion

    #region Factory_Methods
    
    /// <summary>
    /// Creates a new synchronous Entity Framework data transaction.
    /// </summary>
    /// <param name="context">The persistence context to create the transaction for.</param>
    /// <returns>A new EntityFrameworkDataTransaction instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when context is null.</exception>
    internal static EntityFrameworkDataTransaction CreateEntityFrameworkDataTransaction(EntityFrameworkPersistenceContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var transaction = context.Context.Database.BeginTransaction();
        return new EntityFrameworkDataTransaction(context, transaction);
    }
    
    /// <summary>
    /// Creates a new asynchronous Entity Framework data transaction.
    /// </summary>
    /// <param name="context">The persistence context to create the transaction for.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation that returns a new EntityFrameworkDataTransaction instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when context is null.</exception>
    internal static async Task<EntityFrameworkDataTransaction> CreateEntityFrameworkDataTransactionAsync(
        EntityFrameworkPersistenceContext context, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var transaction = await context.Context.Database.BeginTransactionAsync(cancellationToken);
        return new EntityFrameworkDataTransaction(context, transaction);
    }
    
    #endregion

    #region Methods
    
    /// <inheritdoc />
    public void Commit() => Transaction.Commit();
    
    /// <inheritdoc />
    public void Rollback() => Transaction.Rollback();
    
    /// <inheritdoc />
    public Task CommitAsync(CancellationToken cancellationToken = default) => 
        Transaction.CommitAsync(cancellationToken);
    
    /// <inheritdoc />
    public Task RollbackAsync(CancellationToken cancellationToken = default) => 
        Transaction.RollbackAsync(cancellationToken);
    
    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    /// <summary>
    /// Releases the managed and unmanaged resources used by the transaction.
    /// </summary>
    /// <param name="isDisposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    private void Dispose(bool isDisposing)
    {
        if (_disposed) return;
        
        if (isDisposing)
        {
            Transaction?.Dispose();
        }
        
        _disposed = true;
    }
    
    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }
    
    /// <summary>
    /// Asynchronously releases the managed and unmanaged resources used by the transaction.
    /// </summary>
    /// <param name="isDisposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    /// <returns>A task representing the asynchronous disposal operation.</returns>
    private async ValueTask DisposeAsync(bool isDisposing)
    {
        if (_disposed) return;
        
        if (isDisposing && Transaction is not null)
        {
            await Transaction.DisposeAsync();
        }
        
        _disposed = true;
    }
    
    #endregion
}
