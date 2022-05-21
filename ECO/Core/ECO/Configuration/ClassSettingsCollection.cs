using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace ECO.Configuration
{
    [ConfigurationCollection(typeof(ClassSettings))]
    public class ClassSettingsCollection : ConfigurationElementCollection
    {
        #region Protected_Properties

        protected override string ElementName
        {
            get
            {
                return "class";
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

        public ClassSettings this[int index]
        {
            get
            {
                if (index >= base.Count)
                {
                    return null;
                }
                else
                {
                    return (ClassSettings)BaseGet(index);
                }
            }
        }

        #endregion

        #region Protected_Methods

        protected override ConfigurationElement CreateNewElement()
        {
            return new ClassSettings();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ClassSettings)element).Type;
        }

        #endregion
    }
}
