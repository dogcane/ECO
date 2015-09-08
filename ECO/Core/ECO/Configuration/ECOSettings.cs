using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace ECO.Configuration
{
    public class ECOSettings : ConfigurationSection
    {
        #region Private_Fields

        private static ConfigurationProperty _ContextType;

        private static ConfigurationProperty _DataProperty;

        #endregion

        #region Public_Properties

        [ConfigurationProperty("contextType", IsRequired = true)]
        public string ContextType
        {
            get
            {
                return (string)this[_ContextType];
            }
        } 

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
            _ContextType = new ConfigurationProperty("contextType", typeof(string));
            _DataProperty = new ConfigurationProperty("data", typeof(DataSettings));
        }

        #endregion
    }
}
