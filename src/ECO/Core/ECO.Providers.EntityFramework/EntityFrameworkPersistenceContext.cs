namespace ECO.Providers.EntityFramework;

using ECO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Entity Framework-specific persistence context that wraps a DbContext and provides ECO persistence operations.
/// </summary>
/// <param name="context">The Entity Framework DbContext to wrap.</param>
/// <param name="persistenceUnit">The persistence unit that owns this context.</param>
/// <param name="logger">Optional logger for this context.</param>
public class EntityFrameworkPersistenceContext(
    DbContext context, 
    IPersistenceUnit persistenceUnit, 
    ILogger<EntityFrameworkPersistenceContext>? logger = null)
    : PersistenceContextBase<EntityFrameworkPersistenceContext>(persistenceUnit, logger)
{
    #region Public_Properties    
    /// <summary>
    /// Gets the underlying Entity Framework DbContext.
    /// </summary>
    public DbContext Context { get; protected set; } = context ?? throw new ArgumentNullException(nameof(context));    
    #endregion

    #region Protected_Methods    
    /// <inheritdoc />
    protected override void OnAttach<T>(IAggregateRoot<T> entity) => Context.Attach(entity);
    /// <inheritdoc />
    protected override void OnRefresh<T>(IAggregateRoot<T> entity) => Context.Entry(entity).Reload();
    /// <inheritdoc />
    protected override void OnDetach<T>(IAggregateRoot<T> entity) => Context.Entry(entity).State = EntityState.Detached;
    /// <inheritdoc />
    protected override IDataTransaction OnBeginTransaction() => 
        EntityFrameworkDataTransaction.CreateEntityFrameworkDataTransaction(this);
    /// <inheritdoc />
    protected override async Task<IDataTransaction> OnBeginTransactionAsync(CancellationToken cancellationToken = default)
        => await EntityFrameworkDataTransaction.CreateEntityFrameworkDataTransactionAsync(this, cancellationToken);
    /// <inheritdoc />
    protected override void OnSaveChanges() => Context.SaveChanges();
    /// <inheritdoc />
    protected override Task OnSaveChangesAsync(CancellationToken cancellationToken = default)
        => Context.SaveChangesAsync(cancellationToken);

    /// <inheritdoc />
    protected override PersistenceState OnGetPersistenceState<T>(IAggregateRoot<T> entity)
    {
        var entry = Context.Entry(entity);
        return entry.State switch
        {
            EntityState.Added => PersistenceState.Transient,
            EntityState.Modified => PersistenceState.Persistent,
            EntityState.Deleted => PersistenceState.Persistent,
            EntityState.Unchanged => PersistenceState.Persistent,
            EntityState.Detached => PersistenceState.Detached,
            _ => PersistenceState.Unknown
        };
    }

    /// <inheritdoc />
    protected override void OnDispose()
    {
        Context?.Dispose();
        base.OnDispose();
    }
    
    #endregion
}
