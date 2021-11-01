using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO.Resources;

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
