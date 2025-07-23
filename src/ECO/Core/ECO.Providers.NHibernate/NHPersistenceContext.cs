namespace ECO.Providers.NHibernate;

using System;
using System.Threading;
using System.Threading.Tasks;
using ECO.Data;
using global::NHibernate;
using Microsoft.Extensions.Logging;

public sealed class NHPersistenceContext(ISession session, IPersistenceUnit persistenceUnit, ILogger<NHPersistenceContext>? logger) : PersistenceContextBase<NHPersistenceContext>(persistenceUnit, logger)
{
    #region Public_Properties
    public ISession Session { get; private set; } = session;
    #endregion

    #region Protected_Methods
    protected override IDataTransaction OnBeginTransaction() => new NHDataTransaction(this);

    protected override async Task<IDataTransaction> OnBeginTransactionAsync(CancellationToken cancellationToken = default) => await Task.FromResult(OnBeginTransaction());

    protected override void OnClose() => Session.Close();

    protected override void OnSaveChanges()
    {
        Session.Flush();
        Session.Clear();
    }
    
    protected override async Task OnSaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await Session.FlushAsync(cancellationToken);
        Session.Clear();
    }

    protected override void OnAttach<T>(IAggregateRoot<T> entity)
    {
        try
        {
            Session.Lock(entity, LockMode.None);
        }
        catch (NonUniqueObjectException)
        {
            entity = Session.Load(entity.GetType(), entity.Identity) as IAggregateRoot<T> ?? throw new ArgumentNullException(nameof(entity));
        }
    }

    protected override void OnDetach<T>(IAggregateRoot<T> entity) => Session.Evict(entity);

    protected override void OnRefresh<T>(IAggregateRoot<T> entity) => Session.Refresh(entity);

    protected override PersistenceState OnGetPersistenceState<T>(IAggregateRoot<T> entity)
        => Session.Contains(entity) ? PersistenceState.Persistent : PersistenceState.Detached;

    protected override void OnDispose()
    {
        Session.Dispose();
        Transaction?.Dispose();
    }

    #endregion
}
