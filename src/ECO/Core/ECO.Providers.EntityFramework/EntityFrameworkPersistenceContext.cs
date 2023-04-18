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

        public EntityFrameworkPersistenceContext(DbContext context, IPersistenceUnit persistenceUnit, ILogger<EntityFrameworkPersistenceContext>? logger = null)
            : base(persistenceUnit, logger) => Context = context;

        #endregion

        #region Protected_Methods

        protected override void OnAttach<T>(IAggregateRoot<T> entity) => Context.Attach(entity);

        protected override void OnRefresh<T>(IAggregateRoot<T> entity) => Context.Entry(entity).Reload();

        protected override void OnDetach<T>(IAggregateRoot<T> entity) => Context.Entry(entity).State = EntityState.Detached;

        protected override IDataTransaction OnBeginTransaction() => EntityFrameworkDataTransaction.CreateEntityFrameworkDataTransaction(this);

        protected override async Task<IDataTransaction> OnBeginTransactionAsync(CancellationToken cancellationToken = default) => await EntityFrameworkDataTransaction.CreateEntityFrameworkDataTransactionAsync(this, cancellationToken);

        protected override void OnSaveChanges() => Context.SaveChanges();

        protected override async Task OnSaveChangesAsync(CancellationToken cancellationToken = default) => await Context.SaveChangesAsync(cancellationToken);

        #endregion
    }
}
