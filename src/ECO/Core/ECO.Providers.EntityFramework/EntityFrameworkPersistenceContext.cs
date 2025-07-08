namespace ECO.Providers.EntityFramework;

using ECO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

public class EntityFrameworkPersistenceContext(DbContext context, IPersistenceUnit persistenceUnit, ILogger<EntityFrameworkPersistenceContext>? logger = null)
    : PersistenceContextBase<EntityFrameworkPersistenceContext>(persistenceUnit, logger)
{
    #region Public_Properties
    public DbContext Context { get; protected set; } = context;
    #endregion

    #region Protected_Methods
    protected override void OnAttach<T>(IAggregateRoot<T> entity) => Context.Attach(entity);

    protected override void OnRefresh<T>(IAggregateRoot<T> entity) => Context.Entry(entity).Reload();

    protected override void OnDetach<T>(IAggregateRoot<T> entity) => Context.Entry(entity).State = EntityState.Detached;

    protected override IDataTransaction OnBeginTransaction() => EntityFrameworkDataTransaction.CreateEntityFrameworkDataTransaction(this);

    protected override Task<IDataTransaction> OnBeginTransactionAsync(CancellationToken cancellationToken = default)
        => EntityFrameworkDataTransaction.CreateEntityFrameworkDataTransactionAsync(this, cancellationToken).ContinueWith(t => (IDataTransaction)t.Result, cancellationToken);

    protected override void OnSaveChanges() => Context.SaveChanges();

    protected override Task OnSaveChangesAsync(CancellationToken cancellationToken = default)
        => Context.SaveChangesAsync(cancellationToken);
    #endregion
}
