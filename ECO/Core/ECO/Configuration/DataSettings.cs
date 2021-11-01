using System.Configuration;

namespace ECO.Configuration
{
    public class DataSettings : ConfigurationElement
    {
        #region Private_Fields

        private static ConfigurationProperty _PersistenceUnitsProperty;

        #endregion

        #region Public_Properties

        [ConfigurationProperty("persistence-units", IsRequired = true, IsDefaultCollection = true)]
        public PersistenceUnitSettingsCollection PersistenceUnits
        {
            get
            {
                return this[_PersistenceUnitsProperty] as PersistenceUnitSettingsCollection;
            }
        }

        #endregion

        #region ~Ctor

        static DataSettings()
        {
            _PersistenceUnitsProperty = new ConfigurationProperty("persistence-units", typeof(PersistenceUnitSettingsCollection));
        }

        #endregion
    }
}
