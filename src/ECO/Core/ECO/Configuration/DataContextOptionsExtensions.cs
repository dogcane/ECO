using ECO.Data;
using Microsoft.Extensions.Configuration;

namespace ECO.Configuration;

/// <summary>
/// Extension methods for configuring <see cref="DataContextOptions"/>.
/// </summary>
public static class DataContextOptionsExtensions
{
    /// <summary>
    /// Enables transaction requirement for the data context options.
    /// </summary>
    /// <param name="dataContextOptions">The data context options to configure.</param>
    /// <returns>The configured <see cref="DataContextOptions"/> instance.</returns>
    public static DataContextOptions RequireTransaction(this DataContextOptions dataContextOptions)
    {
        ArgumentNullException.ThrowIfNull(dataContextOptions);
        dataContextOptions.RequireTransaction = true;
        return dataContextOptions;
    }

    /// <summary>
    /// Configures the data context options using the specified configuration section.
    /// </summary>
    /// <param name="dataContextOptions">The data context options to configure.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The configured <see cref="DataContextOptions"/> instance.</returns>
    public static DataContextOptions UseConfiguration(this DataContextOptions dataContextOptions, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(dataContextOptions);
        ArgumentNullException.ThrowIfNull(configuration);
        ECOOptions options = configuration.GetSection(ECOOptions.ECOConfigurationName).Get<ECOOptions>() ?? throw new NullReferenceException(nameof(options));
        dataContextOptions.PersistenceUnitFactoryOptions += (persistenceUnitFactory, loggerFactory) =>
        {
            foreach (PersistenceUnitOptions unit in options.PersistenceUnits)
            {
                Type unitType;
                IPersistenceUnit persistenceUnit;
                try
                {
                    unitType = Type.GetType(unit.Type) ?? throw new TypeLoadException($"Unable to load type {unit.Type}");
                    persistenceUnit = Activator.CreateInstance(unitType, unit.Name, loggerFactory) as IPersistenceUnit ?? throw new InvalidOperationException($"Unable to create instance of {unitType}");
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
                        Type type = Type.GetType(classType) ?? throw new TypeLoadException($"Unable to load type {classType}");
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
                        Type type = Type.GetType(listenerType) ?? throw new TypeLoadException($"Unable to load type {listenerType}");
                        IPersistenceUnitListener listener = Activator.CreateInstance(type) as IPersistenceUnitListener ?? throw new InvalidOperationException($"Unable to create instance of {type}");
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
