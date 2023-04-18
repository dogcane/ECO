
using ECO.Data;
using NHibernate;
using System;
using nh = NHibernate;

namespace ECO.Providers.NHibernate
{
    public class NHPersistenceManager<T, K> : PersistenceManagerBase<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Ctor

        public NHPersistenceManager(IDataContext dataContext) : base(dataContext)
        {
        }

        #endregion

        #region Protected_Methods

        protected nh.ISession GetCurrentSession() => (PersistenceContext as NHPersistenceContext ?? throw new InvalidCastException(nameof(NHPersistenceContext))).Session;

        protected nh.ITransaction? GetCurrentTransaction() => GetCurrentSession().GetCurrentTransaction();

        #endregion
    }
}
