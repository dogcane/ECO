using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace ECO.Configuration
{
    public class ECOSettings : ConfigurationSection
    {
        #region Private_Fields

        private static ConfigurationProperty _DataProperty;

        #endregion

        #region Public_Properties

        [ConfigurationProperty("data", IsRequired=true)]
        public DataSettings Data
        {
            get
            {
                return base[_DataProperty] as DataSettings;
            }
        }

        #endregion

        #region ~Ctor

        static ECOSettings()
        {
            _DataProperty = new ConfigurationProperty("data", typeof(DataSettings));
        }

        #endregion
    }
}
