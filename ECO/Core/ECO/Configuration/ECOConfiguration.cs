using ECO.Resources;
using System.Configuration;

namespace ECO.Configuration
{
    public static class ECOConfiguration
    {
        #region Properties

        public static ECOSettings Configuration { get; private set; }

        #endregion

        #region Ctor

        static ECOConfiguration()
        {
            Configuration = ConfigurationManager.GetSection("eco") as ECOSettings;
            if (Configuration == null)
            {
                throw new ConfigurationErrorsException(Errors.CONFIG_NOT_FOUND);
            }
        }

        #endregion
    }
}
