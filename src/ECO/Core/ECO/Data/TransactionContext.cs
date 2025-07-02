using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Data
{
    public sealed class TransactionContext : ITransactionContext
    {
        #region Private_Fields

        private bool _disposed = false;

        private readonly IList<IDataTransaction> _Transactions = new List<IDataTransaction>();

        #endregion

        #region Public_Properties

        public Guid TransactionContextId { get; } = Guid.NewGuid();

        public TransactionStatus Status { get; private set; } = TransactionStatus.Alive;

        public bool AutoCommit { get; }

        public IDataContext DataContext { get; private set; }

        #endregion

        #region ~Ctor

        internal TransactionContext(IDataContext dataContext)
            : this(dataContext, false)
        {

        }

        internal TransactionContext(IDataContext dataContext, bool autoCommit)
        {
            DataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            AutoCommit = autoCommit;
        }

        ~TransactionContext()
        {
            Dispose(false);
        }

        #endregion

        #region Public_Methods

        public void EnlistDataTransaction(IDataTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            _Transactions.Add(transaction);
        }

        public void Commit()
        {
            foreach (IDataTransaction tx in _Transactions)
            {
                tx.Commit();
            }
            Status = TransactionStatus.Committed;
        }

        public void Rollback()
        {
            foreach (IDataTransaction tx in _Transactions)
            {
                tx.Rollback();
            }
            Status = TransactionStatus.RolledBack;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            foreach (IDataTransaction tx in _Transactions)
            {
                await tx.CommitAsync(cancellationToken);
            }
            Status = TransactionStatus.Committed;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            foreach (IDataTransaction tx in _Transactions)
            {
                await tx.RollbackAsync(cancellationToken);
            }
            Status = TransactionStatus.RolledBack;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Asynchronously releases all resources used by the TransactionContext.
        /// </summary>
        /// <returns>A ValueTask representing the asynchronous dispose operation</returns>
        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            if (Status == TransactionStatus.Alive)
            {
                if (AutoCommit)
                {
                    await CommitAsync();
                }
                Status = TransactionStatus.Committed;
            }
            
            foreach (IDataTransaction tx in _Transactions)
            {
                await tx.DisposeAsync();
            }
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (_disposed)
                return;

            if (isDisposing)
            {
                if (Status == TransactionStatus.Alive)
                {
                    if (AutoCommit)
                    {
                        Commit();
                    }
                    Status = TransactionStatus.Committed;
                }
                foreach (IDataTransaction tx in _Transactions)
                {
                    tx.Dispose();
                }
                _disposed = true;
            }
        }

        #endregion
    }
}
