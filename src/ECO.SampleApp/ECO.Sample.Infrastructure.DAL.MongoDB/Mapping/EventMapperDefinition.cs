using ECO.Providers.MongoDB.Mappers;
using ECO.Sample.Domain;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Sample.Infrastructure.DAL.MongoDB.Mapping
{
    public class EventMapperDefinition : MapperDefinitionBase<Event>
    {
        protected override void OnBuildMapperDefinition(BsonClassMap<Event> classMap)
        {
            classMap.AutoMap();
            classMap.UnmapProperty("Sessions");
            classMap.MapField("_Sessions");            
        }
    }
}
