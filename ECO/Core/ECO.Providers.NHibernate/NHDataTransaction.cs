using System;
using System.Collections.Generic;
using System.Text;

using nh = NHibernate;

using ECO.Data;
using System.Data;

namespace ECO.Providers.NHibernate
{
    public class NHDataTransaction : IDataTransaction
    {
        #region Public_Properties

        public nh.ITransaction Transaction { get; protected set; }

        public NHPersistenceContext Context { get; protected set; }

        IPersistenceContext IDataTransaction.Context
        {
            get { return Context; }
        }

        #endregion

        #region ~Ctor

        internal NHDataTransaction(NHPersistenceContext context, IsolationLevel? isolationLevel)
        {
            Transaction = isolationLevel.HasValue ? context.Session.BeginTransaction(isolationLevel.Value) : context.Session.BeginTransaction();
            Context = context;
        }

        ~NHDataTransaction()
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
                Context.Session.Flush();
                Transaction.Commit();
            }
            catch (Exception ex)
            {

                try
                {
                    if (Transaction != null)
                    {
                        Transaction.Rollback();
                        Transaction.Dispose();
                    }
                }
                catch (Exception subex)
                {
                    ex = new AggregateException(ex, subex);
                }
                throw ex;
            }
        }

        public void Rollback()
        {
            try
            {
                Context.Session.Clear();
                Transaction.Rollback();
            }
            catch (Exception ex)
            {
                //???
            }
        }

        #endregion
    }
}
