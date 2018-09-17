using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ECO.Data
{
    public class TransactionContext : IDisposable
    {
        #region Private_Fields

        private IList<IDataTransaction> _Transactions;

        private TransactionStatus _Status;

        private IsolationLevel? _IsolationLevel;

        private bool _AutoCommit;        

        #endregion

        #region Public_Properties

        public TransactionStatus Status
        {
            get
            {
                return _Status;
            }
        }

        public bool AutoCommit
        {
            get
            {
                return _AutoCommit;
            }
        }

        public IsolationLevel? IsolationLevel
        {
            get
            {
                return _IsolationLevel;
            }
        }

        #endregion

        #region ~Ctor

        internal TransactionContext(IsolationLevel? isolationLevel = null)
            : this(false, isolationLevel)
        {

        }

        internal TransactionContext(bool autoCommit, IsolationLevel? isolationLevel = null)
        {            
            _Transactions = new List<IDataTransaction>();
            _Status = TransactionStatus.Alive;
            _AutoCommit = autoCommit;
            _IsolationLevel = isolationLevel;
        }

        ~TransactionContext()
        {
            Dispose(false);
        }

        #endregion

        #region Internal_Methods

        internal void AddDataTransaction(IDataTransaction transaction)
        {
            _Transactions.Add(transaction);
        }

        #endregion

        #region Public_Methods

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
