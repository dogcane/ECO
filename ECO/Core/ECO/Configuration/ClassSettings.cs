using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace ECO.Configuration
{
    public class ClassSettings : ConfigurationElement
    {
        #region Private_Fields

        private static ConfigurationProperty _TypeProperty;

        #endregion

        #region Public_Properties

        [ConfigurationProperty("type")]
        public string Type
        {
            get
            {
                return (string)this[_TypeProperty];
            }
        }

        #endregion

        #region ~Ctor

        static ClassSettings()
        {            
            _TypeProperty = new ConfigurationProperty("type", typeof(string));
        }

        #endregion
    }
}
