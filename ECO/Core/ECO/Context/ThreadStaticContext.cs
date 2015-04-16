using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace ECO.Context
{
    public class ThreadStaticContext : IContextProvider
    {
        #region Fields

        [ThreadStatic]
        private static IDictionary<string, object> _ThreadContext;

        #endregion

        #region ~Ctor

        public ThreadStaticContext()
        {
            
        }

        #endregion

        #region Private_Methods

        private void Initialize()
        {
            if (_ThreadContext == null)
            {
                _ThreadContext = new Dictionary<string, object>();
            }
        }

        #endregion

        #region IContextProvider Members

        public object GetContextData(string dataKey)            
        {
            Initialize();
            return _ThreadContext.ContainsKey(dataKey) ? _ThreadContext[dataKey] : null;
        }

        public void SetContextData(string dataKey, object data)
        {
            Initialize();
            _ThreadContext[dataKey] = data;
        }

        #endregion
    }
}
