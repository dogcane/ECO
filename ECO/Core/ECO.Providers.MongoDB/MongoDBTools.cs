using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB;
using MongoDB.Driver;

namespace ECO.Providers.MongoDB
{
    public static class MongoDBTools
    {
        public static MongoCollection SafeGetCollectionForType<T>(this MongoDatabase database)
        {
            Type currentType = typeof(T);
            while(!currentType.BaseType.Equals(typeof(Entity<>)))
            {
                currentType = currentType.BaseType;
            }
            return database.GetCollection(currentType.Name);
        }
    }
}
