using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.Context
{
    public interface IContextProvider
    {
        #region Methods

        object GetContextData(string dataKey);

        void SetContextData(string dataKey, object data);

        #endregion
    }
}
