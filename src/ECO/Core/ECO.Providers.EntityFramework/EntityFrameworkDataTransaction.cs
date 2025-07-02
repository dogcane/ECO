using ECO.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Providers.EntityFramework;

public class EntityFrameworkDataTransaction : IDataTransaction
{
    #region Fields

    protected bool _disposed = false;

    #endregion

    #region IDataTransaction Members

    public IDbContextTransaction Transaction { get; private set; }

    public EntityFrameworkPersistenceContext Context { get; private set; }

    IPersistenceContext IDataTransaction.Context => Context;

    #endregion

    #region ~Ctor

    private EntityFrameworkDataTransaction(EntityFrameworkPersistenceContext context, IDbContextTransaction transaction)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
    }

    ~EntityFrameworkDataTransaction()
    {
        Dispose(false);
    }

    #endregion

    #region Factory_Methods

    internal static EntityFrameworkDataTransaction CreateEntityFrameworkDataTransaction(EntityFrameworkPersistenceContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var transaction = context.Context.Database.BeginTransaction();
        return new EntityFrameworkDataTransaction(context, transaction);
    }

    internal static async Task<EntityFrameworkDataTransaction> CreateEntityFrameworkDataTransactionAsync(EntityFrameworkPersistenceContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var transaction = await context.Context.Database.BeginTransactionAsync(cancellationToken);
        return new EntityFrameworkDataTransaction(context, transaction);
    }

    #endregion

    #region Methods

    public void Commit() => Transaction.Commit();

    public void Rollback() => Transaction.Rollback();

    public async Task CommitAsync(CancellationToken cancellationToken = default) => await Transaction.CommitAsync(cancellationToken);

    public async Task RollbackAsync(CancellationToken cancellationToken = default) => await Transaction.RollbackAsync(cancellationToken);

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
            Transaction?.Dispose();            
        }
        _disposed = true;
    }
    
    public async ValueTask DisposeAsync()
    {
        DisposeAsync(true);
        GC.SuppressFinalize(this);
    }
    
    /// <summary>
    /// Asynchronously releases all resources used by the EntityFrameworkDataTransaction.
    /// </summary>
    /// <returns>A ValueTask representing the asynchronous dispose operation</returns>
    private async ValueTask DisposeAsync(bool isDisposing)
    {
        if (_disposed)
            return;
        if (isDisposing)
        {
            await Transaction?.DisposeAsync();
        }
        _disposed = true;
    }

    #endregion
}
