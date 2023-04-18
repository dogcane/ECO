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
        #region Protected_Properties

        protected IDocumentSession DocumentSession => (PersistenceContext as MartenPersistenceContext ?? throw new InvalidCastException(nameof(DocumentSession))).Session;

        #endregion

        #region Ctor

        protected MartenPersistenceManager(IDataContext dataContext) : base(dataContext)
        {
        }

        #endregion        
    }
}
