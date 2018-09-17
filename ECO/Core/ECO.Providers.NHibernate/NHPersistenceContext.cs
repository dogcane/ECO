using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using nh = NHibernate;
using System.Reflection;

using ECO.Data;
using System.Data;

namespace ECO.Providers.NHibernate
{
    public class NHPersistenceContext : IPersistenceContext
    {
        #region Public_Properties

        public nh.ISession Session { get; protected set; }

        public IDataTransaction Transaction { get; protected set; }

        #endregion

        #region ~Ctor

        public NHPersistenceContext(nh.ISession session)
        {
            Session = session;
        }

        ~NHPersistenceContext()
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
                else
                {
                    Session.Flush();
                }
                Session.Close();
                GC.SuppressFinalize(this);
            }
            Transaction = null;
            Session = null;
        }

        #endregion

        #region Public_Methods

        public void Attach<T, K>(T entity)  where T : IAggregateRoot<K>            
        {
            try
            {
                Session.Lock(entity, nh.LockMode.None);
            }
            catch (nh.NonUniqueObjectException)
            {
                entity = Session.Load<T>(entity.Identity);
            }
        }

        public void Detach<T, K>(T entity) where T : IAggregateRoot<K>
        {
            Session.Evict(entity);
        }

        public void Refresh<T, K>(T entity) where T : IAggregateRoot<K>
        {
            Session.Refresh(entity);
        }

        public PersistenceState GetPersistenceState<T, K>(T entity) where T : IAggregateRoot<K>
        {
            if (Session.Contains(entity))
            {
                return PersistenceState.Persistent;
            }
            else
            {
                return PersistenceState.Detached;
            }
        }

        public IDataTransaction BeginTransaction()
        {
            Transaction = new NHDataTransaction(this, null);
            return Transaction;
        }

        public IDataTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            Transaction = new NHDataTransaction(this, isolationLevel);
            return Transaction;
        }

        public void Close()
        {
            Dispose(true);
        }

        public void SaveChanges()
        {
            Session.Flush();
            Session.Clear();
        }

        #endregion
    }
}
