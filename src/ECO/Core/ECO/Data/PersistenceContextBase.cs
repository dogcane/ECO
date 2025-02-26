using Microsoft.Extensions.Logging;

namespace ECO.Data;

public abstract class PersistenceContextBase<P>(IPersistenceUnit persistenceUnit, ILogger<P>? logger = null) : IPersistenceContext
    where P : PersistenceContextBase<P>
{
    #region Protected_Fields

    protected bool _disposed = false;

    protected readonly ILogger<P>? _Logger = logger;

    #endregion
    #region Ctor

    ~PersistenceContextBase()
    {
        Dispose(false);
    }

    #endregion

    #region Protected_Methods

    protected virtual IDataTransaction OnBeginTransaction() => new NullDataTransaction(this);

    protected virtual async Task<IDataTransaction> OnBeginTransactionAsync(CancellationToken cancellationToken = default) => await Task.FromResult(OnBeginTransaction());

    protected virtual void OnClose()
    {

    }

    protected virtual void OnDispose()
    {

    }

    protected virtual void OnSaveChanges()
    {

    }
    protected virtual async Task OnSaveChangesAsync(CancellationToken cancellationToken = default) => await Task.Run(OnSaveChanges, cancellationToken);

    protected virtual void OnAttach<T>(IAggregateRoot<T> entity)
    {

    }

    protected virtual void OnDetach<T>(IAggregateRoot<T> entity)
    {

    }

    protected virtual void OnRefresh<T>(IAggregateRoot<T> entity)
    {

    }

    protected virtual PersistenceState OnGetPersistenceState<T>(IAggregateRoot<T> entity) => PersistenceState.Unknown;

    #endregion

    #region IPersistenceContext Membri di

    public Guid PersistenceContextId { get; } = Guid.NewGuid();

    public IPersistenceUnit PersistenceUnit { get; } = persistenceUnit ?? throw new ArgumentNullException(nameof(persistenceUnit));

    public IDataTransaction? Transaction { get; private set; } = null;

    public void Attach<T>(IAggregateRoot<T> entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _Logger?.LogDebug("Attaching entity {entityName}:{entityId} in {name}:{id}", entity.GetType().Name, entity.Identity, PersistenceUnit.Name, PersistenceContextId);
        OnAttach(entity);
    }

    public void Detach<T>(IAggregateRoot<T> entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _Logger?.LogDebug("Detaching entity {entityName}:{entityId} in {name}:{id}", entity.GetType().Name, entity.Identity, PersistenceUnit.Name, PersistenceContextId);
        OnDetach(entity);
    }

    public void Refresh<T>(IAggregateRoot<T> entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _Logger?.LogDebug("Refreshing entity {entityName}:{entityId} in {name}:{id}", entity.GetType().Name, entity.Identity, PersistenceUnit.Name, PersistenceContextId);
        OnRefresh(entity);
    }

    public PersistenceState GetPersistenceState<T>(IAggregateRoot<T> entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _Logger?.LogDebug("Get Persistence State for entity {entityName}:{entityId} in {name}:{id}", entity.GetType().Name, entity.Identity, PersistenceUnit.Name, PersistenceContextId);
        return OnGetPersistenceState(entity);
    }

    public IDataTransaction BeginTransaction()
    {
        _Logger?.LogDebug("Begin transaction in {name}:{id}", PersistenceUnit.Name, PersistenceContextId);
        Transaction = OnBeginTransaction();
        return Transaction;
    }

    public async Task<IDataTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _Logger?.LogDebug("Begin transaction in {name}:{id}", PersistenceUnit.Name, PersistenceContextId);
        Transaction = await OnBeginTransactionAsync(cancellationToken);
        return Transaction;
    }

    public void Close()
    {
        OnClose();
        Dispose(true);
    }

    public void SaveChanges()
    {
        _Logger?.LogDebug("Save changes in {name}:{id}", PersistenceUnit.Name, PersistenceContextId);
        OnSaveChanges();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        _Logger?.LogDebug("Save changes in {name}:{id}", PersistenceUnit.Name, PersistenceContextId);
        await OnSaveChangesAsync(cancellationToken);
    }

    #endregion

    #region IDisposable Membri di

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
            OnDispose();
        }
        _disposed = true;
    }

    #endregion
}
