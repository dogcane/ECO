using ECO.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace ECO.Data
{
    public class PersistenceUnitFactory : IPersistenceUnitFactory
    {
        #region Private_Fields

        private IDictionary<string, IPersistenceUnit> _Units = new Dictionary<string, IPersistenceUnit>();

        private IDictionary<Type, IPersistenceUnit> _Classes = new Dictionary<Type, IPersistenceUnit>();

        #endregion

        #region ~Ctor

        public PersistenceUnitFactory(ECOOptions options)
        {
            foreach (PersistenceUnitOptions unit in options.PersistenceUnits)
            {
                IPersistenceUnit MyUnit = Activator.CreateInstance(Type.GetType(unit.Type)) as IPersistenceUnit;
                MyUnit.Initialize(unit.Name, unit.Attributes);
                _Units.Add(unit.Name, MyUnit);
                foreach (string classType in unit.Classes)
                {
                    try
                    {
                        Type type = Type.GetType(classType);
                        MyUnit.AddClass(type);
                    }
                    catch (Exception ex)
                    {
                        throw new ConfigurationErrorsException($"Error when loading the type {classType}", ex);
                    }
                }
                foreach (string unitListenerType in unit.UnitListeners)
                {
                    try
                    {
                        IPersistenceUnitListener listener = Activator.CreateInstance(Type.GetType(unitListenerType)) as IPersistenceUnitListener;
                        MyUnit.AddUnitListener(listener);
                    }
                    catch (Exception ex)
                    {
                        throw new ConfigurationErrorsException($"Error when loading the type {unitListenerType}", ex);
                    }
                }
            }
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
                throw new PersistenceUnitNotFoundException(name);
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
            else
            {
                foreach (Type registeredType in _Classes.Keys)
                {
                    if (entityType.IsSubclassOf(registeredType))
                    {
                        IPersistenceUnit persistenceUnit = GetPersistenceUnit(registeredType);
                        return persistenceUnit;
                    }
                }
            }
            throw new PersistentClassNotRegisteredException(entityType);
        }

        #endregion
    }
}
