using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ECO.Data
{
    public sealed class PersistenceUnitFactory : IPersistenceUnitFactory
    {
        #region Private_Fields

        private readonly IDictionary<string, IPersistenceUnit> _Units = new Dictionary<string, IPersistenceUnit>();

        private readonly IDictionary<Type, IPersistenceUnit> _Classes = new Dictionary<Type, IPersistenceUnit>();

        private readonly ILoggerFactory _LoggerFactory;

        private readonly ILogger<PersistenceUnitFactory> _Logger;

        #endregion

        #region ~Ctor

        public PersistenceUnitFactory(ILoggerFactory loggerFactory = null)
        {
            _LoggerFactory = loggerFactory;
            _Logger = _LoggerFactory?.CreateLogger<PersistenceUnitFactory>();
        }

        #endregion

        #region Public_Methods

        public IPersistenceUnitFactory AddPersistenceUnit(IPersistenceUnit persistenceUnit)
        {
            _Units.Add(persistenceUnit.Name, persistenceUnit);
            foreach (var classType in persistenceUnit.Classes)
            {
                _Classes.Add(classType, persistenceUnit);
            }
            return this;
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
