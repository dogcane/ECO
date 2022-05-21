using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

using ECO.Data;

namespace ECO.Providers.MongoDB
{
    public class MongoPersistenceUnit : PersistenceUnitBase
    {
        #region Consts

        private static readonly string CONNECTIONSTRING_ATTRIBUTE = "connectionString";

        private static readonly string DATABASE_ATTRIBUTE = "database";

        private static readonly string SERIALIZERS_DATETIME_ATTRIBUTE = "serializers.datetimeutc";

        private static readonly string SERIALIZERS_IDENTITYMAP_ATTRIBUTE = "serializers.identitymap";

        #endregion

        #region Private_Structs

        private struct SerializersUse
        {
            public bool IdentityMap;
            public bool DateTimeUtc;
        }

        #endregion

        #region Fields

        private MongoDatabase _Database;

        private SerializersUse _SerializersUse;
        
        #endregion

        #region Protected_Methods

        protected virtual void OnSetup(MongoDatabase database)
        {

        }    

        protected override void OnInitialize(IDictionary<string, string> extendedAttributes)
        {
            base.OnInitialize(extendedAttributes);
            _SerializersUse = new SerializersUse();
            if (!extendedAttributes.ContainsKey(CONNECTIONSTRING_ATTRIBUTE))
            {
                throw new ApplicationException(string.Format("The attribute '{0}' was not found in the persistent unit configuration", CONNECTIONSTRING_ATTRIBUTE));
            }
            if (!extendedAttributes.ContainsKey(DATABASE_ATTRIBUTE))
            {
                throw new ApplicationException(string.Format("The attribute '{0}' was not found in the persistent unit configuration", DATABASE_ATTRIBUTE));
            }
            if (extendedAttributes.ContainsKey(SERIALIZERS_DATETIME_ATTRIBUTE))
            {
                _SerializersUse.DateTimeUtc = "1".Equals(extendedAttributes[SERIALIZERS_DATETIME_ATTRIBUTE]);
            }
            if (extendedAttributes.ContainsKey(SERIALIZERS_IDENTITYMAP_ATTRIBUTE))
            {
                _SerializersUse.IdentityMap = "1".Equals(extendedAttributes[SERIALIZERS_IDENTITYMAP_ATTRIBUTE]);
            }
            ConventionRegistry.Register("ECO-Identity",
                new ConventionPack() { new Conventions.ECOMapConvention() },
                type => type.GetProperty("Identity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly) != null);
            _Database = new MongoClient(extendedAttributes[CONNECTIONSTRING_ATTRIBUTE])
                .GetServer()
                .GetDatabase(extendedAttributes[DATABASE_ATTRIBUTE]);
            if (_SerializersUse.DateTimeUtc)
            {
                BsonSerializer.RegisterSerializer(typeof(DateTime), new Serializers.DateTimeUtcSerializer());
            }            
            OnSetup(_Database);
        }

        protected override IPersistenceContext CreateContext()
        {
            return new MongoPersistenceContext(_Database);
        }        

        public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>()
        {
            return new MongoReadOnlyRepository<T, K>();
        }

        public override IRepository<T, K> BuildRepository<T, K>()
        {
            return new MongoRepository<T, K>();
        }

        protected override void OnClassAdded(Type classType)
        {
            base.OnClassAdded(classType);
            if (_SerializersUse.IdentityMap)
            {
                BsonSerializer.RegisterSerializer(classType, Serializers.IdentityMapSerializer.Instance);
            }
        }

        #endregion
    }
}
