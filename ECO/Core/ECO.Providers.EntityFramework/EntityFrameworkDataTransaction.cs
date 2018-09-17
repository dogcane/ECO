using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO.Data;

namespace ECO.Providers.EntityFramework
{
    public class EntityFrameworkDataTransaction : IDataTransaction
    {
        #region IDataTransaction Members

        public DbContextTransaction Transaction { get; protected set; }

        public EntityFrameworkPersistenceContext Context { get; protected set; }

        IPersistenceContext IDataTransaction.Context
        {
            get { return Context; }
        }

        #endregion

        #region ~Ctor

        internal EntityFrameworkDataTransaction(EntityFrameworkPersistenceContext context, IsolationLevel? isolationLevel)
        {
            Transaction = isolationLevel.HasValue ? context.Context.Database.BeginTransaction(isolationLevel.Value) : context.Context.Database.BeginTransaction();
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
            if (isDisposing)
            {
                if (Transaction != null)
                {
                    Transaction.Dispose();
                }
                GC.SuppressFinalize(this);
            }
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
