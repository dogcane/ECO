using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace ECO.Providers.MongoDB.Serializers
{
    /// <summary>
    /// ??? WORK IN PROGRESS
    /// </summary>
    public class ECOSerializer : BsonBaseSerializer
    {
        #region Private_Methods

        private MongoIdentityMap GetCurrentIdentityMap(Type nominalType)
        {
            return ((MongoPersistenceContext)ECO.Data.PersistenceUnitFactory.Instance.GetPersistenceUnit(nominalType).GetCurrentContext()).IdentityMap;
        }

        private object GetECOIdentity(object @entity)
        {
            var property = @entity.GetType().GetProperty("Identity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            return property.GetValue(@entity);
        }

        #endregion

        #region Properties

        private static ECOSerializer _Instance = new ECOSerializer();

        public static ECOSerializer Instance
        {
            get { return _Instance; }
        }

        #endregion

        #region IBsonSerializer Members

        public override object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
        {
            var entityMap = GetCurrentIdentityMap(nominalType);
            var entity = base.Deserialize(bsonReader, nominalType, options);
            var identity = GetECOIdentity(entity);
            var entityFromMap = entityMap[identity];
            if (entityFromMap == null)
            {
                entityMap[identity] = entity;
            }
            return entityFromMap ?? entity;
        }

        public override object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
        {
            var entityMap = GetCurrentIdentityMap(nominalType);
            var entity = base.Deserialize(bsonReader, nominalType, actualType, options);
            var identity = GetECOIdentity(entity);
            var entityFromMap = entityMap[identity];
            if (entityFromMap == null)
            {
                entityMap[identity] = entity;
            }
            return entityFromMap ?? entity;
        }

        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            base.Serialize(bsonWriter, nominalType, value, options);
            GetCurrentIdentityMap(nominalType)[GetECOIdentity(value)] = value;
        }

        #endregion
    }
}
