using System;
using System.Collections.Generic;

namespace ECO.Data
{
    public sealed class TransactionContext : ITransactionContext
    {
        #region Private_Fields

        private IList<IDataTransaction> _Transactions;

        private TransactionStatus _Status;

        private bool _AutoCommit;

        #endregion

        #region Public_Properties

        public TransactionStatus Status => _Status;

        public bool AutoCommit => _AutoCommit;

        #endregion

        #region ~Ctor

        internal TransactionContext()
            : this(false)
        {

        }

        internal TransactionContext(bool autoCommit)
        {
            _Transactions = new List<IDataTransaction>();
            _Status = TransactionStatus.Alive;
            _AutoCommit = autoCommit;
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
            _Status = TransactionStatus.Committed;
        }

        public void Rollback()
        {
            foreach (IDataTransaction tx in _Transactions)
            {
                tx.Rollback();
            }
            _Status = TransactionStatus.RolledBack;
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