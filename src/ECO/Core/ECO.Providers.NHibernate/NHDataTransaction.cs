using ECO.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using nh = NHibernate;

namespace ECO.Providers.NHibernate
{
    public class NHDataTransaction : IDataTransaction
    {
        #region Fields

        private bool _disposed = false;

        #endregion

        #region Public_Properties

        public nh.ITransaction Transaction { get; protected set; }

        public NHPersistenceContext Context { get; protected set; }

        IPersistenceContext IDataTransaction.Context
        {
            get { return Context; }
        }

        #endregion

        #region ~Ctor

        internal NHDataTransaction(NHPersistenceContext context)
        {
            Transaction = context.Session.BeginTransaction();
            Context = context;
        }

        ~NHDataTransaction()
        {
            Dispose(false);
        }

        public void Dispose() => Dispose(true);

        /// <summary>
        /// Asynchronously releases all resources used by the NHDataTransaction.
        /// </summary>
        /// <returns>A ValueTask representing the asynchronous dispose operation</returns>
        public ValueTask DisposeAsync()
        {
            if (_disposed)
                return ValueTask.CompletedTask;

            if (Transaction != null)
            {
                // NHibernate ITransaction does not support async disposal,
                // so we fall back to synchronous disposal
                Transaction.Dispose();
            }
            _disposed = true;
            GC.SuppressFinalize(this);
            return ValueTask.CompletedTask;
        }

        private void Dispose(bool isDisposing)
        {
            if (_disposed)
                return;

            if (isDisposing)
            {
                Transaction?.Dispose();
                GC.SuppressFinalize(this);
            }
            _disposed = true;
        }

        #endregion

        #region Public_Methods

        public void Commit() => Transaction.Commit();

        public void Rollback() => Transaction.Rollback();

        public async Task CommitAsync(CancellationToken cancellationToken = default) => await Transaction.CommitAsync(cancellationToken);

        public async Task RollbackAsync(CancellationToken cancellationToken = default) => await Transaction.RollbackAsync(cancellationToken);

        #endregion
    }
}
