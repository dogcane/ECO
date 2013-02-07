using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace ECO.Configuration
{
    [ConfigurationCollection(typeof(AttributeSettings))]
    public class AttributeSettingsCollection : ConfigurationElementCollection
    {
        #region Protected_Properties

        protected override string ElementName
        {
            get
            {
                return "attribute";
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

        public AttributeSettings this[int index]
        {
            get
            {
                if (index >= base.Count)
                {
                    return null;
                }
                else
                {
                    return (AttributeSettings)BaseGet(index);
                }
            }
        }

        #endregion

        #region Protected_Methods

        protected override ConfigurationElement CreateNewElement()
        {
            return new AttributeSettings();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AttributeSettings)element).Name;
        }

        #endregion
    }
}
