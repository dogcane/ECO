using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECO.Data;

namespace ECO.Providers.InMemory
{
    public class InMemoryTransaction : IDataTransaction
    {
        #region Ctor

        public InMemoryTransaction(InMemoryPersistenceContext context)
        {
            Context = context;
        }

        #endregion

        #region IDataTransaction Membri di

        public IPersistenceContext Context { get; protected set; }

        public void Commit()
        {

        }

        public void Rollback()
        {

        }

        #endregion

        #region IDisposable Membri di

        public void Dispose()
        {

        }

        #endregion
    }
}
