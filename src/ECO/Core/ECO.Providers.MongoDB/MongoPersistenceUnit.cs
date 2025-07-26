namespace ECO.Providers.MongoDB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ECO.Data;
using ECO.Providers.MongoDB.Configuration;
using ECO.Providers.MongoDB.Conventions;
using ECO.Providers.MongoDB.Mappers;
using global::MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public sealed class MongoPersistenceUnit(string name, ILoggerFactory loggerFactory) : PersistenceUnitBase<MongoPersistenceUnit>(name, loggerFactory)
{
    #region Consts
    private static readonly string CONNECTIONSTRING_ATTRIBUTE = "connectionString";
    private static readonly string DATABASE_ATTRIBUTE = "database";
    private static readonly string MAPPINGASSEMBLIES_ATTRIBUTE = "mappingAssemblies";
    private static readonly string SERIALIZERS_IDENTITYMAP_ATTRIBUTE = "useIdentityMap";
    #endregion

    #region Fields
    private IMongoDatabase? _Database;
    private MongoDBOptions? _Options;
    #endregion

    #region Internal_Methods

    /// <summary>
    /// Configures the persistence unit with the provided options.
    /// This method is called by the configuration extension.
    /// </summary>
    /// <param name="options">The MongoDB configuration options.</param>
    internal void ConfigureWith(MongoDBOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _Options = options;

        // Validate required options
        if (string.IsNullOrWhiteSpace(options.ConnectionString))
            throw new ApplicationException("ConnectionString is required when using fluent MongoDB configuration.");
        if (string.IsNullOrWhiteSpace(options.DatabaseName))
            throw new ApplicationException("DatabaseName is required when using fluent MongoDB configuration.");

        // Add listeners from options
        foreach (var listener in options.Listeners)
        {
            AddUnitListener(listener);
        }

        // Initialize MongoDB components
        InitializeMongoDB(options.ConnectionString, options.DatabaseName, options.MappingAssemblies, options.UseIdentityMap);

        // Add aggregate types from assembly discovery
        foreach (var markerType in options.AssemblyMarkerTypes)
        {
            var aggregateTypes = DiscoverAggregateTypesFromAssembly(markerType.Assembly);
            foreach (var aggregateType in aggregateTypes)
            {
                AddClass(aggregateType);
            }
        }

        // Add explicit aggregate types
        foreach (var aggregateType in options.AggregateTypes)
        {
            AddClass(aggregateType);
        }
    }

    #endregion

    #region Private_Methods

    /// <summary>
    /// Discovers aggregate root types from the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan for aggregate root types.</param>
    /// <returns>Collection of aggregate root types found in the assembly.</returns>
    private static IEnumerable<Type> DiscoverAggregateTypesFromAssembly(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAggregateRoot<>)));
    }

    /// <summary>
    /// Initializes MongoDB components with the provided configuration.
    /// </summary>
    /// <param name="connectionString">The MongoDB connection string.</param>
    /// <param name="databaseName">The MongoDB database name.</param>
    /// <param name="mappingAssemblies">The mapping assemblies string (optional).</param>
    /// <param name="useIdentityMap">Whether to use identity map for serializers.</param>
    private void InitializeMongoDB(string connectionString, string databaseName, string? mappingAssemblies, bool useIdentityMap)
    {
        //Conventions
        ECOIdentityMapConvention.Register();
        
        //Mappers
        if (!string.IsNullOrWhiteSpace(mappingAssemblies))
        {
            var mappingDefinitions = mappingAssemblies
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(Assembly.Load)
                .SelectMany(asm => asm.ExportedTypes)
                .Where(tp => typeof(IMapperDefinition).IsAssignableFrom(tp))
                .Select(tp => Activator.CreateInstance(tp) as IMapperDefinition);
            foreach (var mappingDefinition in mappingDefinitions)
                mappingDefinition?.BuildMapperDefition();
        }
        
        //Serializers
        if (useIdentityMap)
        {
            //TODO...
        }
        
        _Database = new MongoClient(connectionString).GetDatabase(databaseName);
    }

    #endregion

    #region Protected_Methods   

    protected override void OnInitialize(IDictionary<string, string> extendedAttributes, IConfiguration configuration)
    {
        base.OnInitialize(extendedAttributes, configuration);
        
        // Skip initialization if already configured via fluent API
        if (_Options != null)
            return;
            
        if (!extendedAttributes.TryGetValue(CONNECTIONSTRING_ATTRIBUTE, out string? connectionString) || string.IsNullOrWhiteSpace(connectionString))
            throw new ApplicationException($"The attribute '{CONNECTIONSTRING_ATTRIBUTE}' was not found in the persistent unit configuration or it is empty");
        if (!extendedAttributes.TryGetValue(DATABASE_ATTRIBUTE, out string? databaseName) || string.IsNullOrWhiteSpace(databaseName))
            throw new ApplicationException($"The attribute '{DATABASE_ATTRIBUTE}' was not found in the persistent unit configuration or it is empty");
        
        extendedAttributes.TryGetValue(MAPPINGASSEMBLIES_ATTRIBUTE, out string? mappingAssemblies);
        bool useIdentityMap = extendedAttributes.TryGetValue(SERIALIZERS_IDENTITYMAP_ATTRIBUTE, out var useIdentityMapStr) && 
                              bool.TryParse(useIdentityMapStr, out var useMap) && useMap;
        
        InitializeMongoDB(connectionString, databaseName, mappingAssemblies, useIdentityMap);
    }

    protected override IPersistenceContext OnCreateContext() => new MongoPersistenceContext(_Database!, this, _LoggerFactory?.CreateLogger<MongoPersistenceContext>());

    #endregion

    #region Public_Methods
    public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext) => new MongoRepository<T, K>(dataContext);
    public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext) => new MongoRepository<T, K>(dataContext);
    #endregion
}
