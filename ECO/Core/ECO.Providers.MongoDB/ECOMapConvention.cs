using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson.Serialization.Conventions;
using System.Reflection;

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
            classMap.SetIdMember(new global::MongoDB.Bson.Serialization.BsonMemberMap(classMap, classMap.ClassType.GetProperty("Identity")));
            classMap.SetIgnoreExtraElements(true);
        }
    }
}
