using ECO.Data;
using ECO.Providers.MongoDB.Conventions;
using ECO.Providers.MongoDB.Mappers;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ECO.Providers.MongoDB
{
    public sealed class MongoPersistenceUnit : PersistenceUnitBase<MongoPersistenceUnit>
    {
        #region Consts

        private static readonly string CONNECTIONSTRING_ATTRIBUTE = "connectionString";

        private static readonly string DATABASE_ATTRIBUTE = "database";

        private static readonly string MAPPINGASSEMBLIES_ATTRIBUTE = "mappingAssemblies";

        private static readonly string SERIALIZERS_IDENTITYMAP_ATTRIBUTE = "useIdentityMap";

        #endregion

        #region Ctor

        public MongoPersistenceUnit(string name, ILoggerFactory loggerFactory) : base(name, loggerFactory)
        {

        }

        #endregion

        #region Fields

        private IMongoDatabase _Database;

        #endregion

        #region Protected_Methods   

        protected override void OnInitialize(IDictionary<string, string> extendedAttributes)
        {
            base.OnInitialize(extendedAttributes);
            if (!extendedAttributes.ContainsKey(CONNECTIONSTRING_ATTRIBUTE))
            {
                throw new ApplicationException(string.Format("The attribute '{0}' was not found in the persistent unit configuration", CONNECTIONSTRING_ATTRIBUTE));
            }
            if (!extendedAttributes.ContainsKey(DATABASE_ATTRIBUTE))
            {
                throw new ApplicationException(string.Format("The attribute '{0}' was not found in the persistent unit configuration", DATABASE_ATTRIBUTE));
            }
            //Conventions
            ECOIdentityMapConvention.Register();
            //Mappers
            if (extendedAttributes.ContainsKey(MAPPINGASSEMBLIES_ATTRIBUTE))
            {
                var mappingDefinitions = extendedAttributes[MAPPINGASSEMBLIES_ATTRIBUTE]
                    .Split(";", StringSplitOptions.RemoveEmptyEntries)
                    .Select(asm => Assembly.Load(asm))
                    .SelectMany(asm => asm.ExportedTypes)
                    .Where(tp => typeof(IMapperDefinition).IsAssignableFrom(tp))
                    .Select(tp => Activator.CreateInstance(tp) as IMapperDefinition);
                foreach (var mappingDefinition in mappingDefinitions)
                {
                    mappingDefinition.BuildMapperDefition();
                }
            }
            //Serializers
            if (extendedAttributes.ContainsKey(SERIALIZERS_IDENTITYMAP_ATTRIBUTE) && bool.Parse(extendedAttributes[SERIALIZERS_IDENTITYMAP_ATTRIBUTE]))
            {
                //TODO...
            }
            _Database = new MongoClient(extendedAttributes[CONNECTIONSTRING_ATTRIBUTE]).GetDatabase(extendedAttributes[DATABASE_ATTRIBUTE]);
        }

        protected override IPersistenceContext OnCreateContext() => new MongoPersistenceContext(_Database, this, _LoggerFactory.CreateLogger<MongoPersistenceContext>());

        public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext) => new MongoRepository<T, K>(dataContext);

        public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext) => new MongoRepository<T, K>(dataContext);

        #endregion
    }
}
