using ECO.Resources;
using System;

namespace ECO.Data
{
    public class DataContextClosedException : ApplicationException
    {
        #region Ctor

        public DataContextClosedException()
            : base(Errors.DATA_CONTEXT_CLOSED)
        {

        }

        #endregion
    }
}
