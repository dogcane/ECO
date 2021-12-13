using ECO.Data;
using System;
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

        private void Dispose(bool isDisposing)
        {
            if (_disposed)
                return;

            if (isDisposing)
            {
                if (Transaction != null)
                {
                    Transaction.Dispose();
                }
                GC.SuppressFinalize(this);
            }
            _disposed = true;
            Transaction = null;
            Context = null;
        }

        #endregion

        #region Public_Methods

        public void Commit() => Transaction.Commit();

        public void Rollback() => Transaction.Rollback();

        #endregion
    }
}
