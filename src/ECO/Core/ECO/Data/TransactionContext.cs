using System;
using System.Collections.Generic;

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

        #endregion

        #region ~Ctor

        internal TransactionContext()
            : this(false)
        {

        }

        internal TransactionContext(bool autoCommit) => AutoCommit = autoCommit;

        ~TransactionContext()
        {
            Dispose(false);
        }

        #endregion

        #region Public_Methods

        public void EnlistDataTransaction(IDataTransaction transaction) => _Transactions.Add(transaction);

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

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
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
