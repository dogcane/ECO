using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace ECO.Configuration
{
    [ConfigurationCollection(typeof(PersistenceUnitSettingsCollection))]
    public class PersistenceUnitSettingsCollection : ConfigurationElementCollection
    {
        #region Protected_Properties

        protected override string ElementName
        {
            get
            {
                return "persistence-unit";
            }
        }

        #endregion

        #region Public_Properties

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        public PersistenceUnitSettings this[int index]
        {
            get
            {
                if (index >= base.Count)
                {
                    return null;
                }
                else
                {
                    return (PersistenceUnitSettings)BaseGet(index);
                }
            }
        }

        #endregion

        #region Protected_Methods

        protected override ConfigurationElement CreateNewElement()
        {
            return new PersistenceUnitSettings();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PersistenceUnitSettings)element).Name;
        }

        #endregion
    }
}
