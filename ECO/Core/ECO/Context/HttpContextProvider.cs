using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ECO.Context
{
    public class HttpContextProvider : IContextProvider
    {
        #region ~Ctor

        public HttpContextProvider()
        {

        }

        #endregion

        #region IContextProvider Members

        public object GetContextData(string dataKey)
        {
            return HttpContext.Current.Items[dataKey];
        }

        public void SetContextData(string dataKey, object data)
        {
            HttpContext.Current.Items[dataKey] = data;
        }

        #endregion
    }
}
