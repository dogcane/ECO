using System;
using System.Collections.Generic;

namespace ECO.Data
{
    public interface IPersistenceUnitFactory
    {
        IPersistenceUnit BuildPersistenceUnit<T>(string name) where T : IPersistenceUnit;
        IPersistenceUnit BuildPersistenceUnit<T>(string name, IDictionary<string, string> extendedAttributes) where T : IPersistenceUnit;
        IPersistenceUnit GetPersistenceUnit(string name);
        IPersistenceUnit GetPersistenceUnit(Type entityType);
        IPersistenceUnit GetPersistenceUnit<T>();
    }
}