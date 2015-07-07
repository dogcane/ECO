using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Data
{
    public class NullDataTransaction : IDataTransaction
    {
        #region Ctor

        public NullDataTransaction(IPersistenceContext context)
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
