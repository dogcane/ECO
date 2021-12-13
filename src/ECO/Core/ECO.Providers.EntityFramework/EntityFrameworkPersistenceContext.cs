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

        public EntityFrameworkPersistenceContext(DbContext context, IPersistenceUnit persistenceUnit, ILogger<EntityFrameworkPersistenceContext> logger = null)
            : base(persistenceUnit, logger) => Context = context;

        #endregion

        #region Public_Methods

        protected override IDataTransaction OnBeginTransaction() => new EntityFrameworkDataTransaction(this);

        protected override void OnSaveChanges() => Context.SaveChanges();

        #endregion
    }
}
