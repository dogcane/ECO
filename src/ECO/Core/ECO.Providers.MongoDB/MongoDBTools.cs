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
    }
}
