using ECO.Data;
using Microsoft.Extensions.Configuration;
using System;

namespace ECO.Configuration
{
    public static class DataContextOptionsExtensions
    {
        public static DataContextOptions RequireTransaction(this DataContextOptions dataContextOptions)
        {
            dataContextOptions.RequireTransaction = true;
            return dataContextOptions;
        }

        public static DataContextOptions UsingConfiguration(this DataContextOptions dataContextOptions, IConfiguration configuration)
        {
            ECOOptions options = configuration.GetSection(ECOOptions.ECOConfigurationName).Get<ECOOptions>();
            dataContextOptions.PersistenceUnitFactoryOptions += (persistenceUnitFactory, loggerFactory) =>
            {
                foreach (PersistenceUnitOptions unit in options.PersistenceUnits)
                {
                    Type unitType = Type.GetType(unit.Type);
                    IPersistenceUnit persistenceUnit = (IPersistenceUnit)Activator.CreateInstance(unitType, unit.Name, loggerFactory);
                    persistenceUnit.Initialize(unit.Attributes);
                    if (unit.Classes != null)
                    {
                        foreach (string classType in unit.Classes)
                        {
                            try
                            {
                                Type type = Type.GetType(classType);
                                persistenceUnit.AddClass(type);
                            }
                            catch (Exception ex)
                            {
                                throw new ApplicationException($"Error when loading the type {classType}", ex);
                            }
                        }
                    }
                    if (unit.Listeners != null)
                    {
                        foreach (string listenerType in unit.Listeners)
                        {
                            try
                            {
                                Type type = Type.GetType(listenerType);
                                IPersistenceUnitListener listener = (IPersistenceUnitListener)Activator.CreateInstance(type);
                                persistenceUnit.AddUnitListener(listener);
                            }
                            catch (Exception ex)
                            {
                                throw new ApplicationException($"Error when loading the type {listenerType}", ex);
                            }
                        }
                    }
                    persistenceUnitFactory.AddPersistenceUnit(persistenceUnit);
                }
            };
            return dataContextOptions;
        }
    }
}
