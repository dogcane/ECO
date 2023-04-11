
using ECO.Data;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using nh = NHibernate;

namespace ECO.Providers.NHibernate
{
    public sealed class NHPersistenceContext : PersistenceContextBase<NHPersistenceContext>
    {
        #region Public_Properties

        public nh.ISession Session { get; private set; }

        #endregion

        #region ~Ctor

        public NHPersistenceContext(nh.ISession session, IPersistenceUnit persistenceUnit, ILogger<NHPersistenceContext> logger) : base(persistenceUnit, logger)
        {
            Session = session;
        }

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
                Session.Lock(entity, nh.LockMode.None);
            }
            catch (nh.NonUniqueObjectException)
            {   
                entity = Session.Load(entity.GetType(), entity.Identity) as IAggregateRoot<T>;
            }
        }

        protected override void OnDetach<T>(IAggregateRoot<T> entity) => Session.Evict(entity);

        protected override void OnRefresh<T>(IAggregateRoot<T> entity) => Session.Refresh(entity);

        protected override PersistenceState OnGetPersistenceState<T>(IAggregateRoot<T> entity)
        {
            if (Session.Contains(entity))
            {
                return PersistenceState.Persistent;
            }
            else
            {
                return PersistenceState.Detached;
            }
        }

        protected override void OnDispose()
        {
            Session.Dispose();
            if (Transaction != null) Transaction.Dispose();
        }

        #endregion
    }
}
