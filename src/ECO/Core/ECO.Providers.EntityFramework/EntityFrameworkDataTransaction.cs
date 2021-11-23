using ECO.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace ECO.Providers.EntityFramework
{
    public sealed class EntityFrameworkDataTransaction : IDataTransaction
    {
        #region Fields

        private bool _disposed = false;

        #endregion

        #region IDataTransaction Members

        public IDbContextTransaction Transaction { get; private set; }

        public EntityFrameworkPersistenceContext Context { get; private set; }

        IPersistenceContext IDataTransaction.Context
        {
            get { return Context; }
        }

        #endregion

        #region ~Ctor

        internal EntityFrameworkDataTransaction(EntityFrameworkPersistenceContext context)
        {
            Transaction = context.Context.Database.BeginTransaction();
            Context = context;
        }

        ~EntityFrameworkDataTransaction()
        {
            Dispose(false);
        }

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

        public void Commit()
        {
            try
            {
                Transaction.Commit();
            }
            catch (Exception ex)
            {
                if (Transaction != null)
                {
                    Transaction.Rollback();
                    Transaction.Dispose();
                }
                throw ex;
            }
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

        #endregion
    }
}
