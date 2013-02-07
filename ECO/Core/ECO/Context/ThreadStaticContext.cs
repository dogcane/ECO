using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace ECO.Context
{
    public class ThreadStaticContext : IContextProvider
    {

        #region ~Ctor

        public ThreadStaticContext()
        {
            
        }

        #endregion

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
