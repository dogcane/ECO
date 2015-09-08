using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

using ECO;
using ECO.Configuration;
using ECO.Resources;
using System.Reflection;
using ECO.Context;

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
            TryLoadConfig();
        }

        #endregion

        #region Private_Methods

        private void TryLoadConfig()
        {
            ECOSettings settings = ECOConfiguration.Configuration;
            foreach (PersistenceUnitSettings unit in settings.Data.PersistenceUnits)
            {
                IPersistenceUnit MyUnit = Activator.CreateInstance(Type.GetType(unit.Type)) as IPersistenceUnit;
                MyUnit.Initialize(unit.Name, GetExtendedAttributes(unit.Attributes));
                MyUnit.ClassAdded += MyUnit_ClassAdded;
                MyUnit.ClassRemoved += MyUnit_ClassRemoved;
                _Units.Add(unit.Name, MyUnit);                    
                foreach (ClassSettings setting in unit.Classes)
                {
                    try
                    {
                        Type type = Type.GetType(setting.Type);
                        MyUnit.AddClass(type);
                    }
                    catch (Exception ex)
                    {
                        throw new ConfigurationErrorsException(string.Format(Errors.TYPE_LOAD_EXCEPTION, setting.Type), ex);
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
                        throw new ConfigurationErrorsException(string.Format(Errors.TYPE_LOAD_EXCEPTION, setting.Type), ex);
                    }
                }
            }
        }

        private void MyUnit_ClassAdded(object sender, PersistentUnitClassEventArgs e)
        {
            if (!_Classes.ContainsKey(e.ClassType))
            {
                _Classes.Add(e.ClassType, e.PersistenceUnit);
            }
        }

        private void MyUnit_ClassRemoved(object sender, PersistentUnitClassEventArgs e)
        {
            if (_Classes.ContainsKey(e.ClassType))
            {
                _Classes.Remove(e.ClassType);
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

        public IPersistenceUnit BuildPersistenceUnit<T>(string name) where T : IPersistenceUnit
        {
            return BuildPersistenceUnit<T>(name, null);
        }

        public IPersistenceUnit BuildPersistenceUnit<T>(string name, IDictionary<string, string> extendedAttributes) where T : IPersistenceUnit
        {
            if (!_Units.ContainsKey(name))
            {
                IPersistenceUnit MyUnit = Activator.CreateInstance<T>();
                if (extendedAttributes != null && extendedAttributes.Count > 0)
                {
                    MyUnit.Initialize(name, extendedAttributes);
                }
                MyUnit.ClassAdded += MyUnit_ClassAdded;
                MyUnit.ClassRemoved += MyUnit_ClassRemoved;
                _Units.Add(name, MyUnit);
            }
            return _Units[name];
        }

        public IPersistenceUnit GetPersistenceUnit(string name)
        {
            if (_Units.ContainsKey(name))
            {
                return _Units[name];
            }
            else
            {
                throw new PersistenceUnitNotFound(name);
            }
        }

        public IPersistenceUnit GetPersistenceUnit<T>()
        {
            return GetPersistenceUnit(typeof(T));
        }

        public IPersistenceUnit GetPersistenceUnit(Type entityType)
        {
            if (_Classes.ContainsKey(entityType))
            {
                return _Classes[entityType];
            }
            else if (entityType.GetCustomAttributes<PersistenceContextAttribute>().Any())
            {
                PersistenceContextAttribute attribute = entityType.GetCustomAttribute<PersistenceContextAttribute>();
                IPersistenceUnit persistenceUnit = GetPersistenceUnit(attribute.PersistenceContextName);
                persistenceUnit.AddClass(entityType);
                return persistenceUnit;
            }
            else
            {
                foreach (Type registeredType in _Classes.Keys)
                {
                    if (entityType.IsSubclassOf(registeredType))
                    {
                        IPersistenceUnit persistenceUnit = GetPersistenceUnit(registeredType);
                        persistenceUnit.AddClass(entityType);
                        return persistenceUnit;
                    }
                }
            }
            throw new PersistentClassNotRegistered(entityType);
        }

        #endregion
    }
}
