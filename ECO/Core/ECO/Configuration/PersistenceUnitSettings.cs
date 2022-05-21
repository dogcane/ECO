using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace ECO.Configuration
{
    public class PersistenceUnitSettings : ConfigurationElement
    {
        #region Private_Fields

        private static ConfigurationProperty _NameProperty;

        private static ConfigurationProperty _TypeProperty;        

        private static ConfigurationProperty _ListenersProperty;

        private static ConfigurationProperty _ClassesProperty;

        private static ConfigurationProperty _AttributesProperty;

        #endregion

        #region Public_Propertues

        [ConfigurationProperty("name", IsRequired=true, IsKey=true)]
        public string Name
        {
            get
            {
                return (string)this[_NameProperty];
            }
        }

        [ConfigurationProperty("type", IsRequired=true)]
        public string Type
        {
            get
            {
                return (string)this[_TypeProperty];
            }
        }        

        [ConfigurationProperty("listeners", IsRequired = false)]
        public UnitListenerSettingsCollection Listeners
        {
            get
            {
                return (UnitListenerSettingsCollection)this[_ListenersProperty];
            }
        }

        [ConfigurationProperty("classes", IsRequired=true)]
        public ClassSettingsCollection Classes
        {
            get
            {
                return (ClassSettingsCollection)this[_ClassesProperty];
            }
        }

        [ConfigurationProperty("attributes", IsRequired=false)]
        public AttributeSettingsCollection Attributes
        {
            get
            {
                return (AttributeSettingsCollection)this[_AttributesProperty];
            }
        }

        #endregion

        #region ~Ctor

        static PersistenceUnitSettings()
        {
            _NameProperty = new ConfigurationProperty("name", typeof(string));
            _TypeProperty = new ConfigurationProperty("type", typeof(string));            
            _ListenersProperty = new ConfigurationProperty("listeners", typeof(UnitListenerSettingsCollection));
            _ClassesProperty = new ConfigurationProperty("classes", typeof(ClassSettingsCollection));
            _AttributesProperty = new ConfigurationProperty("attributes", typeof(AttributeSettingsCollection));
        }

        #endregion
    }
}
