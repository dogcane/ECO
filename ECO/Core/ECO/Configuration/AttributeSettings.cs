using System.Configuration;

namespace ECO.Configuration
{
    public class AttributeSettings : ConfigurationElement
    {
        #region Private_Fields

        private static ConfigurationProperty _NameProperty;

        private static ConfigurationProperty _ValueProperty;

        #endregion

        #region Public_Properties

        [ConfigurationProperty("name")]
        public string Name
        {
            get
            {
                return (string)base[_NameProperty];
            }
        }

        [ConfigurationProperty("value")]
        public string Value
        {
            get
            {
                return (string)base[_ValueProperty];
            }
        }

        #endregion

        #region ~Ctor

        static AttributeSettings()
        {
            _NameProperty = new ConfigurationProperty("name", typeof(string));
            _ValueProperty = new ConfigurationProperty("value", typeof(string));
        }

        #endregion
    }
}
