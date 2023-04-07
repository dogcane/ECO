using ECO.Data;
using Marten;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.Providers.Marten
{
    public abstract class MartenPersistenceManager<T, K> : PersistenceManagerBase<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Ctor

        protected MartenPersistenceManager(IDataContext dataContext) : base(dataContext)
        {
        }

        #endregion

        #region Protected_Methods

        protected IDocumentSession GetCurrentSession() => (PersistenceContext as MartenPersistenceContext)!.Session;

        #endregion
    }
}
