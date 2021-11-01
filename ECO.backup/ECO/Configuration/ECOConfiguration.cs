using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO.Resources;

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
