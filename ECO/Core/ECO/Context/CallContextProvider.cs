using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Context
{
    public class CallContextProvider : IContextProvider
    {
        #region IContextProvider Members

        public object GetContextData(string dataKey)
        {
            return CallContext.GetData(dataKey);
        }

        public void SetContextData(string dataKey, object data)
        {
            CallContext.SetData(dataKey, data);
        }

        #endregion
    }
}
