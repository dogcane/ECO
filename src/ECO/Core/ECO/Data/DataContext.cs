using Microsoft.Extensions.Logging;

namespace ECO.Data;

public sealed class DataContext : IDisposable, IDataContext
{
    #region Private_Fields

    private bool _disposed = false;

    private readonly IPersistenceUnitFactory _PersistenceUnitFactory;

    private readonly ILogger<DataContext>? _Logger;

    private readonly Dictionary<string, IPersistenceContext> _Contexts = [];

    #endregion

    #region Public_Properties

    public Guid DataContextId { get; } = Guid.NewGuid();

    public ITransactionContext? Transaction { get; private set; } = null;

    #endregion

    #region ~Ctor

    public DataContext(IPersistenceUnitFactory persistenceUnitFactory, ILogger<DataContext>? logger = null)
    {
        _PersistenceUnitFactory = persistenceUnitFactory ?? throw new ArgumentNullException(nameof(persistenceUnitFactory));
        _Logger = logger;
        _Logger?.LogDebug("Data context '{DataContextId}' is opening", DataContextId);
    }

    ~DataContext()
    {
        Dispose(false);
    }

    #endregion

    #region Protected_Methods

    private IPersistenceContext InitializeOrReturnContext(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);
        IPersistenceUnit persistenceUnit = _PersistenceUnitFactory.GetPersistenceUnit(entityType);
        string persistenceUnitName = persistenceUnit.Name;
        if (_Contexts.TryGetValue(persistenceUnitName, out IPersistenceContext? value))
        {
            _Logger?.LogDebug("Persistence context for entity {entityType} already initialized => '{persistenceUnitName}':'{persistenceContextId}'", entityType, persistenceUnitName, value.PersistenceContextId);
            return value;
        }
        else
        {
            _Logger?.LogDebug("Initialization of persistence context for entity '{entityType}'", entityType);
            IPersistenceContext context = persistenceUnit.CreateContext();
            _Contexts.Add(persistenceUnitName, context);
            Transaction?.EnlistDataTransaction(context.BeginTransaction());
            _Logger?.LogDebug("Persistence context for entity {entityType} initialized => '{persistenceUnitName}':'{persistenceContextId}'", entityType, persistenceUnitName, context.PersistenceContextId);
            return context;
        }
    }

    #endregion

    #region Public_Methods

    public ITransactionContext BeginTransaction() => BeginTransaction(false);

    public ITransactionContext BeginTransaction(bool autoCommit)
    {
        if (Transaction == null || Transaction?.Status != TransactionStatus.Alive)
        {
            Transaction = new TransactionContext(this, autoCommit);
            _Logger?.LogDebug("Starting a new transaction context '{transactionContextId}'", Transaction.TransactionContextId);
            foreach (IPersistenceContext persistenceContext in _Contexts.Values)
            {
                IDataTransaction tx = persistenceContext.BeginTransaction();
                Transaction.EnlistDataTransaction(tx);
            }
            return Transaction;
        }
        else
        {
            throw new InvalidOperationException($"There is already an active transaction context with id '{Transaction?.TransactionContextId}'");
        }
    }

    public async Task<ITransactionContext> BeginTransactionAsync(CancellationToken cancellationToken = default) => await BeginTransactionAsync(false, cancellationToken);

    public async Task<ITransactionContext> BeginTransactionAsync(bool autoCommit, CancellationToken cancellationToken = default)
    {
        if (Transaction == null || Transaction?.Status != TransactionStatus.Alive)
        {
            Transaction = new TransactionContext(this, autoCommit);
            _Logger?.LogDebug("Starting a new transaction context '{transactionContextId}'", Transaction.TransactionContextId);
            foreach (IPersistenceContext persistenceContext in _Contexts.Values)
            {
                IDataTransaction tx = await persistenceContext.BeginTransactionAsync(cancellationToken);
                Transaction.EnlistDataTransaction(tx);
            }
            return Transaction;
        }
        else
        {
            throw new InvalidOperationException($"There is already an active transaction context with id '{Transaction?.TransactionContextId}'");
        }
    }

    public void Attach<T>(IAggregateRoot<T> entity) => GetCurrentContext(entity).Attach(entity);

    public void Detach<T>(IAggregateRoot<T> entity) => GetCurrentContext(entity).Detach(entity);

    public void Refresh<T>(IAggregateRoot<T> entity) => GetCurrentContext(entity).Refresh(entity);

    public PersistenceState GetPersistenceState<T>(IAggregateRoot<T> entity) => GetCurrentContext(entity).GetPersistenceState(entity);

    public IPersistenceContext GetCurrentContext<T>() => GetCurrentContext(typeof(T));

    public IPersistenceContext GetCurrentContext(object entity) => GetCurrentContext(entity.GetType());

    public IPersistenceContext GetCurrentContext(Type entityType) => InitializeOrReturnContext(entityType);

    public void SaveChanges()
    {
        foreach (IPersistenceContext persistenceContext in _Contexts.Values)
        {
            persistenceContext.SaveChanges();
        }
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (IPersistenceContext persistenceContext in _Contexts.Values)
        {
            await persistenceContext.SaveChangesAsync(cancellationToken);
        }
    }


    #endregion

    #region IDisposable Members

    public void Close()
    {
        _Logger?.LogDebug("Data context '{DataContextId}' is closing", DataContextId);
        Dispose();
    }

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
            _Logger?.LogDebug("Data context '{DataContextId}' is disposing", DataContextId);
            Transaction?.Dispose();
            foreach (IPersistenceContext persistenceContext in _Contexts.Values)
            {
                persistenceContext.Dispose();
            }
            _disposed = true;
        }
    }

    #endregion
}
