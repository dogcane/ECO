using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization.Conventions;

namespace ECO.Providers.MongoDB
{
    public class ECOMapConvention : IClassMapConvention
    {
        public string Name
        {
            get { return "ECOIdentity"; }
        }

        public void Apply(global::MongoDB.Bson.Serialization.BsonClassMap classMap)
        {
            classMap.MapIdMember(classMap.ClassType.GetProperty("Identity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly));
        }
    }
}
