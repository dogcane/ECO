using System;

namespace ECO.Data
{
    public interface IPersistenceUnitFactory
    {
        IPersistenceUnitFactory AddPersistenceUnit(IPersistenceUnit persistenceUnit);
        IPersistenceUnit GetPersistenceUnit(string name);
        IPersistenceUnit GetPersistenceUnit(Type entityType);
        IPersistenceUnit GetPersistenceUnit<T>();
        IDataContext OpenDataContext();
    }
}