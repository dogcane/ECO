using ECO.Providers.MongoDB.Mappers;
using ECO.Sample.Domain;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Sample.Infrastructure.DAL.MongoDB.Mapping
{
    public class SessionMapperDefinition : MapperDefinitionBase<Session>
    {
        protected override void OnBuildMapperDefinition(BsonClassMap<Session> classMap)
        {
            classMap.AutoMap();
            classMap.UnmapProperty(s => s.Event);            
        }
    }
}
