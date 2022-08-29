using MongoDB.Bson.Serialization;

namespace ECO.Providers.MongoDB.Mappers
{
    public abstract class MapperDefinitionBase<T> : IMapperDefinition
    {
        public void BuildMapperDefition() => BsonClassMap.RegisterClassMap<T>(cm => OnBuildMapperDefinition(cm));

        protected abstract void OnBuildMapperDefinition(BsonClassMap<T> classMap);
    }
}
