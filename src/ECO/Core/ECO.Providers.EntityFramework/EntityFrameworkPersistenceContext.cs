using ECO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECO.Providers.EntityFramework
{
    public class EntityFrameworkPersistenceContext : PersistenceContextBase<EntityFrameworkPersistenceContext>
    {
        #region Public_Properties

        public DbContext Context { get; protected set; }

        #endregion

        #region ~Ctor

        public EntityFrameworkPersistenceContext(IPersistenceUnit persistenceUnit, ILogger<EntityFrameworkPersistenceContext> logger, DbContext context)
            : base(persistenceUnit, logger)
        {
            Context = context;
        }

        #endregion

        #region Public_Methods

        protected override IDataTransaction OnBeginTransaction()
        {
            return new EntityFrameworkDataTransaction(this);
        }

        protected override void OnSaveChanges()
        {
            base.OnSaveChanges();
            Context.SaveChanges();
        }

        protected override void OnClose()
        {
            base.OnClose();
            Context.SaveChanges();
        }

        #endregion
    }
}
