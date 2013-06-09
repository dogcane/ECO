using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using System.Reflection;

namespace ECO.Providers.MongoDB
{
    public static class MongoDBTools
    {
        public static MongoCollection SafeGetCollectionForType<T>(this MongoDatabase database)
        {
            return database.GetCollection(typeof(T).Name);
        }

        public static void MapECOIdentity<T, K>(this BsonClassMap<T> bsonClassMap, IIdGenerator generator)
            where T : IAggregateRoot<K>
        {
            bsonClassMap.AutoMap();
            var idmap = new BsonMemberMap(bsonClassMap, typeof(T).GetProperty("Identity"));
            bsonClassMap.SetIdMember(idmap);
            bsonClassMap.SetIgnoreExtraElements(true);
        }
    }
}
