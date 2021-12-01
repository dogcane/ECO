
using ECO.Data;
using Microsoft.Extensions.Logging;
using nh = NHibernate;

namespace ECO.Providers.NHibernate
{
    public sealed class NHPersistenceContext : PersistenceContextBase<NHPersistenceContext>
    {
        #region Public_Properties

        public nh.ISession Session { get; private set; }

        #endregion

        #region ~Ctor

        public NHPersistenceContext(nh.ISession session, IPersistenceUnit persistenceUnit, ILoggerFactory loggerFactory) : base(persistenceUnit, loggerFactory.CreateLogger<NHPersistenceContext>())
        {
            Session = session;
        }

        #endregion

        #region Protected_Methods

        protected override IDataTransaction OnBeginTransaction() => new NHDataTransaction(this);

        protected override void OnClose() => Session.Close();

        protected override void OnSaveChanges()
        {
            Session.Flush();
            Session.Clear();
        }

        protected override void OnAttach<T, K>(T entity)
        {
            try
            {
                Session.Lock(entity, nh.LockMode.None);
            }
            catch (nh.NonUniqueObjectException)
            {
                entity = Session.Load<T>(entity.Identity);
            }
        }

        protected override void OnDetach<T, K>(T entity) => Session.Evict(entity);

        protected override void OnRefresh<T, K>(T entity) => Session.Refresh(entity);

        protected override PersistenceState OnGetPersistenceState<T, K>(T entity)
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
