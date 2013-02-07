using System;
using System.Collections.Generic;
using System.Text;


namespace ECO.Context
{
    public sealed class ApplicationContext
    {
        #region Private_Properties

        private static IContextProvider RealContextProvider
        {
            get
            {
                if (System.Web.HttpContext.Current != null)
                {
                    return new HttpContextProvider();
                }
                else
                {
                    return new ThreadStaticContext();
                }
            }
        }

        #endregion

        #region Public_Methods

        public static object GetContextData(string dataKey)
        {
            return RealContextProvider.GetContextData(dataKey);
        }

        public static void SetContextData(string dataKey, object data)
        {
            RealContextProvider.SetContextData(dataKey, data);
        }

        #endregion
    }
}
