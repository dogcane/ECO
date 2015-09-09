using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO.Context;

namespace ECO.Integrations.Owin
{
    public class OwinContextProvider : IContextProvider
    {
        #region IContextProvider Members

        public object GetContextData(string dataKey)
        {
            throw new NotImplementedException();
        }

        public void SetContextData(string dataKey, object data)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
