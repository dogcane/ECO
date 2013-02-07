using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

using ECO;
using ECO.Configuration;

namespace ECO.Data
{
    public class PersistenceUnitFactory
    {
        #region Private_Fields

        private static PersistenceUnitFactory _Instance;

        private static object _SyncLock = new object();

        private IDictionary<string, IPersistenceUnit> _Units = new Dictionary<string, IPersistenceUnit>();

        private IDictionary<Type, IPersistenceUnit> _Classes = new Dictionary<Type, IPersistenceUnit>();

        #endregion

        #region Public_Methods

        public static PersistenceUnitFactory Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_SyncLock)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new PersistenceUnitFactory();
                        }
                    }
                }
                return _Instance;
            }
        }

        #endregion

        #region ~Ctor

        private PersistenceUnitFactory()
        {
            LoadConfig();
        }

        #endregion

        #region Private_Methods

        private void LoadConfig()
        {
            ECOSettings settings = ConfigurationManager.GetSection("eco") as ECOSettings;
            foreach (PersistenceUnitSettings unit in settings.Data.PersistenceUnits)
            {
                IPersistenceUnit MyUnit = Activator.CreateInstance(Type.GetType(unit.Type)) as IPersistenceUnit;
                MyUnit.Initialize(unit.Name, GetExtendedAttributes(unit.Attributes));
                _Units.Add(unit.Name, MyUnit);
                foreach (ClassSettings setting in unit.Classes)
                {
                    try
                    {
                        Type type = Type.GetType(setting.Type);
                        _Classes.Add(type, MyUnit);
                        MyUnit.AddClass(type);
                    }
                    catch (Exception ex)
                    {
                        throw new ConfigurationErrorsException(string.Format("Il tipo '{0}' non è stato trovato nella configurazione di ECO", setting.Type), ex);
                    }
                }
                foreach (UnitListenerSettings setting in unit.Listeners)
                {
                    try
                    {
                        IPersistenceUnitListener listener = Activator.CreateInstance(Type.GetType(setting.Type)) as IPersistenceUnitListener;
                        MyUnit.AddUnitListener(listener);
                    }
                    catch (Exception ex)
                    {
                        throw new ConfigurationErrorsException(string.Format("Il tipo '{0}' non è stato trovato nella configurazione di ECO", setting.Type), ex);
                    }
                }
            }
        }

        private IDictionary<string, string> GetExtendedAttributes(AttributeSettingsCollection attributes)
        {
            Dictionary<string, string> extendedAttributes = new Dictionary<string, string>();
            foreach (AttributeSettings setting in attributes)
            {
                extendedAttributes.Add(setting.Name, setting.Value);
            }
            return extendedAttributes;
        }

        #endregion

        #region Public_Methods

        public IPersistenceUnit GetPersistenceUnit(string name)
        {
            if (_Units.ContainsKey(name))
            {
                return _Units[name];
            }
            else
            {
                throw new ApplicationException();
            }
        }

        public IPersistenceUnit GetPersistenceUnit(Type entityType)
        {
            if (_Classes.ContainsKey(entityType))
            {
                return _Classes[entityType];
            }
            else
            {
                foreach (Type registeredType in _Classes.Keys)
                {
                    if (entityType.IsSubclassOf(registeredType))
                    {
                        _Classes.Add(entityType, _Classes[registeredType]);
                        return _Classes[entityType];
                    }
                }
            }
            throw new PersistentClassNotRegistered(entityType);
        }

        #endregion
    }
}
