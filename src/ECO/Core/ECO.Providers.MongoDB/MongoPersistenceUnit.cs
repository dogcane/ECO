namespace ECO.Providers.MongoDB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ECO.Data;
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
    #endregion

    #region Protected_Methods   

    protected override void OnInitialize(IDictionary<string, string> extendedAttributes, IConfiguration configuration)
    {
        base.OnInitialize(extendedAttributes, configuration);
        if (!extendedAttributes.TryGetValue(CONNECTIONSTRING_ATTRIBUTE, out string? connectionString) || string.IsNullOrWhiteSpace(connectionString))
            throw new ApplicationException($"The attribute '{CONNECTIONSTRING_ATTRIBUTE}' was not found in the persistent unit configuration or it is empty");
        if (!extendedAttributes.TryGetValue(DATABASE_ATTRIBUTE, out string? databaseName) || string.IsNullOrWhiteSpace(databaseName))
            throw new ApplicationException($"The attribute '{DATABASE_ATTRIBUTE}' was not found in the persistent unit configuration or it is empty");
        //Conventions
        ECOIdentityMapConvention.Register();
        //Mappers
        if (extendedAttributes.TryGetValue(MAPPINGASSEMBLIES_ATTRIBUTE, out string? mappingAssemblies) && !string.IsNullOrWhiteSpace(mappingAssemblies))
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
        if (extendedAttributes.TryGetValue(SERIALIZERS_IDENTITYMAP_ATTRIBUTE, out var useIdentityMap) && bool.TryParse(useIdentityMap, out var useMap) && useMap)
        {
            //TODO...
        }
        _Database = new MongoClient(connectionString).GetDatabase(databaseName);
    }

    protected override IPersistenceContext OnCreateContext() => new MongoPersistenceContext(_Database!, this, _LoggerFactory?.CreateLogger<MongoPersistenceContext>());

    #endregion

    #region Public_Methods
    public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext) => new MongoRepository<T, K>(dataContext);
    public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext) => new MongoRepository<T, K>(dataContext);
    #endregion
}
