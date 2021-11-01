using System.Configuration;

namespace ECO.Configuration
{
    public class UnitListenerSettings : ConfigurationElement
    {
        #region Private_Fields

        private static ConfigurationProperty _TypeProperty;

        #endregion

        #region Public_Properties

        [ConfigurationProperty("type", IsRequired = true, IsKey = true)]
        public string Type
        {
            get
            {
                return (string)this[_TypeProperty];
            }
        }

        #endregion

        #region Ctor

        static UnitListenerSettings()
        {
            _TypeProperty = new ConfigurationProperty("type", typeof(string));
        }

        #endregion
    }
}
