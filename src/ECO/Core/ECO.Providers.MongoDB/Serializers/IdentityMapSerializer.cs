//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using ECO.Data;
//using MongoDB.Bson.IO;
//using MongoDB.Bson.Serialization;
//using MongoDB.Bson.Serialization.Serializers;

//namespace ECO.Providers.MongoDB.Serializers
//{
//    /// <summary>
//    /// ??? WORK IN PROGRESS
//    /// </summary>
//    public class IdentityMapSerializer<T, K> : ClassSerializerBase<T> where T : class, IAggregateRoot<K>
//    {

//        #region Ctor

//        public IdentityMapSerializer()
//        {
//        }

//        #endregion

//        #region Private_Methods

//        private MongoIdentityMap GetCurrentIdentityMap(Type nominalType)
//        {            
//            return ((MongoPersistenceContext)ECO.Data.PersistenceUnitFactory.Instance.GetPersistenceUnit(nominalType).GetCurrentContext()).IdentityMap;
//        }

//        private object GetECOIdentity(object @entity)
//        {
//            var property = @entity.GetType().GetProperty("Identity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
//            return property.GetValue(@entity);
//        }

//        #endregion

//        #region Properties

//        private static IdentityMapSerializer _Instance = new IdentityMapSerializer();

//        public static IdentityMapSerializer Instance
//        {
//            get { return _Instance; }
//        }

//        public Type ValueType => throw new NotImplementedException();

//        #endregion

//        #region IBsonSerializer Members

//        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
//        {
//            throw new NotImplementedException();
//        }

//        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
//        {
//            throw new NotImplementedException();
//        }

//        /*
//        public override object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
//        {
//            var entityMap = GetCurrentIdentityMap(nominalType);
//            var entity = base.Deserialize(bsonReader, nominalType, actualType, options);
//            var identity = GetECOIdentity(entity);
//            var entityFromMap = entityMap[identity];
//            if (entityFromMap == null)
//            {
//                entityMap[identity] = entity;
//            }
//            return entityFromMap ?? entity;
//        }

//        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
//        {
//            base.Serialize(bsonWriter, nominalType, value, options);
//            GetCurrentIdentityMap(nominalType)[GetECOIdentity(value)] = value;
//        }
//        */

//        #endregion
//    }
//}
