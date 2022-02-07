using ECO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

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

        protected override IDataTransaction OnBeginTransaction() => EntityFrameworkDataTransaction.CreateEntityFrameworkDataTransaction(this);

        protected override async Task<IDataTransaction> OnBeginTransactionAsync(CancellationToken cancellationToken = default) => await EntityFrameworkDataTransaction.CreateEntityFrameworkDataTransactionAsync(this, cancellationToken);

        protected override void OnSaveChanges() => Context.SaveChanges();

        protected override async Task OnSaveChangesAsync(CancellationToken cancellationToken = default) => await Context.SaveChangesAsync(cancellationToken);

        #endregion
    }
}
