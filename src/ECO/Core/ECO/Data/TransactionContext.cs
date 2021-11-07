using System;
using System.Collections.Generic;

namespace ECO.Data
{
    public sealed class TransactionContext : ITransactionContext
    {
        #region Private_Fields

        private readonly IList<IDataTransaction> _Transactions;

        #endregion

        #region Public_Properties

        public Guid TransactionContextId { get; }

        public TransactionStatus Status { get; private set; }

        public bool AutoCommit { get; }

        #endregion

        #region ~Ctor

        internal TransactionContext()
            : this(false)
        {

        }

        internal TransactionContext(bool autoCommit)
        {
            _Transactions = new List<IDataTransaction>();
            TransactionContextId = Guid.NewGuid();
            Status = TransactionStatus.Alive;
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

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (Status == TransactionStatus.Alive)
                {
                    if (AutoCommit)
                    {
                        Commit();
                    }
                    else
                    {
                        Rollback();
                    }
                }
            }
        }

        #endregion
    }
}
