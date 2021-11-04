using ECO.Configuration;
using Microsoft.Extensions.Options;
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

        public PersistenceUnitFactory(IOptions<ECOOptions> optionsAccessor)
        {
            var options = optionsAccessor.Value;
            foreach (PersistenceUnitOptions unit in options.PersistenceUnits)
            {
                IPersistenceUnit persistenceUnit = Activator.CreateInstance(Type.GetType(unit.Type)) as IPersistenceUnit;
                persistenceUnit.Initialize(unit.Name, unit.Attributes);
                _Units.Add(unit.Name, persistenceUnit);
                if (unit.Classes != null)
                {
                    foreach (string classType in unit.Classes)
                    {
                        try
                        {
                            Type type = Type.GetType(classType);
                            persistenceUnit.AddClass(type);
                            _Classes.Add(type, persistenceUnit);
                        }
                        catch (Exception ex)
                        {
                            throw new ConfigurationErrorsException($"Error when loading the type {classType}", ex);
                        }
                    }
                }
                if (unit.Listeners != null)
                {
                    foreach (string unitListenerType in unit.Listeners)
                    {
                        try
                        {
                            IPersistenceUnitListener listener = Activator.CreateInstance(Type.GetType(unitListenerType)) as IPersistenceUnitListener;
                            persistenceUnit.AddUnitListener(listener);
                        }
                        catch (Exception ex)
                        {
                            throw new ConfigurationErrorsException($"Error when loading the type {unitListenerType}", ex);
                        }
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
