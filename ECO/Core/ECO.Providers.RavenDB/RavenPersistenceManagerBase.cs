using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Raven.Client;

using ECO;
using ECO.Data;

namespace ECO.Providers.RavenDB
{
    public class RavenPersistenceManagerBase<T, K> : PersistenceManagerBase<T, K>
        where T : IAggregateRoot<K>
    {
        #region Protected_Methods

        protected IAsyncDocumentSession GetCurrentSession()
        {
            return (GetCurrentContext() as RavenPersistenceContext).Session;
        }

        #endregion
    }
}
