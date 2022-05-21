using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using ECO.Configuration;
using ECO.Resources;


namespace ECO.Context
{
    public sealed class ApplicationContext
    {
        #region Ctor

        static ApplicationContext()
        {
            try
            {
                ContextProvider = Activator.CreateInstance(Type.GetType(ECOConfiguration.Configuration.ContextType)) as IContextProvider;
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException(string.Format(Errors.TYPE_LOAD_EXCEPTION, ECOConfiguration.Configuration.ContextType), ex);
            }

        }

        #endregion

        #region Properties

        public static IContextProvider ContextProvider { get; private set; }

        #endregion

        #region Public_Methods

        public static object GetContextData(string dataKey)
        {
            return ContextProvider.GetContextData(dataKey);
        }

        public static void SetContextData(string dataKey, object data)
        {
            ContextProvider.SetContextData(dataKey, data);
        }

        #endregion
    }
}
