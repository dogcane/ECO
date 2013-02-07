using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using nh = NHibernate;

using ECO.Data;

namespace ECO.Providers.NHibernate
{
    public class NHPersistenceManagerBase<T, K> : PersistenceManagerBase<T, K>
        where T : IAggregateRoot<K>
    {
        #region Protected_Methods

        protected nh.ISession GetCurrentSession()
        {
            return (GetCurrentContext() as NHPersistenceContext).Session;
        }

        protected nh.ITransaction GetCurrentTransaction()
        {
            return (GetCurrentContext().Transaction as NHDataTransaction).Transaction;
        }

        #endregion
    }
}
