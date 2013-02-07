using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace ECO.Configuration
{
    [ConfigurationCollection(typeof(UnitListenerSettingsCollection))]
    public class UnitListenerSettingsCollection : ConfigurationElementCollection
    {
        #region Protected_Properties

        protected override string ElementName
        {
            get
            {
                return "listener";
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

        public UnitListenerSettings this[int index]
        {
            get
            {
                if (index >= base.Count)
                {
                    return null;
                }
                else
                {
                    return (UnitListenerSettings)BaseGet(index);
                }
            }
        }

        #endregion

        #region Protected_Methods

        protected override ConfigurationElement CreateNewElement()
        {
            return new UnitListenerSettings();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UnitListenerSettings)element).Type;
        }

        #endregion
    }
}
