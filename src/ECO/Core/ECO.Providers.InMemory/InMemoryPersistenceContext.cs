using ECO.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ECO.Providers.InMemory
{
    public class InMemoryPersistenceContext : PersistentContextBase<InMemoryPersistenceContext>
    {
        #region Ctor

        public InMemoryPersistenceContext(IPersistenceUnit persistenceUnit, ILoggerFactory loggerFactory)
            :base(persistenceUnit, loggerFactory)
        {

        }

        #endregion

        #region Methods

        protected override IDataTransaction OnBeginTransaction()
        {
            return new NullDataTransaction(this);
        }

        #endregion
    }
}
