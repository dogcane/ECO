using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using nh = NHibernate;
using System.Reflection;

using ECO.Data;

namespace ECO.Providers.NHibernate
{
    public class NHPersistenceContext : IPersistenceContext
    {
        #region Private_Fields

        private nh.ISession _Session;

        private NHDataTransaction _Transaction;

        #endregion

        #region Public_Properties

        public nh.ISession Session
        {
            get
            {
                return _Session;
            }
        }

        public IDataTransaction Transaction
        {
            get
            {
                return _Transaction;
            }
        }

        #endregion

        #region ~Ctor

        public NHPersistenceContext(nh.ISession session)
        {
            _Session = session;
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
                if (_Transaction != null)
                {
                    _Transaction.Dispose();
                }
                else
                {
                    _Session.Flush();
                }
                _Session.Close();
                GC.SuppressFinalize(this);
            }
            _Transaction = null;
            _Session = null;
        }

        #endregion

        #region Public_Methods

        public void Attach<T, K>(T entity)  where T : IAggregateRoot<K>            
        {
            try
            {
                _Session.Lock(entity, nh.LockMode.None);
            }
            catch (nh.NonUniqueObjectException) //Fix della madonna
            {
                entity = _Session.Load<T>(entity.Identity);
            }
        }

        public void Detach<T, K>(T entity) where T : IAggregateRoot<K>
        {
            _Session.Evict(entity);
        }

        public void Refresh<T, K>(T entity) where T : IAggregateRoot<K>
        {
            _Session.Refresh(entity);
        }

        public PersistenceState GetPersistenceState<T, K>(T entity) where T : IAggregateRoot<K>
        {
            if (_Session.Contains(entity))
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
            _Transaction = new NHDataTransaction(this);
            return Transaction;
        }

        public void Close()
        {
            Dispose(true);
        }

        public void SaveChanges()
        {
            _Session.Flush();
            _Session.Clear();
        }

        #endregion
    }
}
