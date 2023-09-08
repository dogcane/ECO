using ECO.Data;
using Microsoft.Extensions.Configuration;
using System;

namespace ECO.Configuration
{
    public static class DataContextOptionsExtensions
    {
        public static DataContextOptions RequireTransaction(this DataContextOptions dataContextOptions)
        {
            if (dataContextOptions == null) throw new ArgumentNullException(nameof(dataContextOptions));
            dataContextOptions.RequireTransaction = true;
            return dataContextOptions;
        }

        public static DataContextOptions UseConfiguration(this DataContextOptions dataContextOptions, IConfiguration configuration)
        {
            if (dataContextOptions == null) throw new ArgumentNullException(nameof(dataContextOptions));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            ECOOptions options = configuration.GetSection(ECOOptions.ECOConfigurationName).Get<ECOOptions>() ?? throw new NullReferenceException(nameof(options));
            dataContextOptions.PersistenceUnitFactoryOptions += (persistenceUnitFactory, loggerFactory) =>
            {
                foreach (PersistenceUnitOptions unit in options.PersistenceUnits)
                {
                    Type unitType;
                    IPersistenceUnit persistenceUnit;
                    try
                    {
                        unitType = Type.GetType(unit.Type)!;
                        persistenceUnit = (IPersistenceUnit)Activator.CreateInstance(unitType, unit.Name, loggerFactory)!;
                        persistenceUnit.Initialize(unit.Attributes, configuration);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException($"Error when loading the type {unit.Type}", ex);
                    }
                    
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
                    persistenceUnitFactory.AddPersistenceUnit(persistenceUnit);
                }
            };
            return dataContextOptions;
        }
    }
}
