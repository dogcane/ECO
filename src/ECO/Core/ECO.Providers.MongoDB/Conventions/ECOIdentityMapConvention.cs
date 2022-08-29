using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using System.Reflection;

namespace ECO.Providers.MongoDB.Conventions
{
    public class ECOIdentityMapConvention : IClassMapConvention
    {
        public string Name
        {
            get { return "ECOMapConvention"; }
        }

        public void Apply(BsonClassMap classMap)
        {
            classMap.MapIdProperty("Identity");
        }

        public static void Register()
        {
            ConventionRegistry.Register(
                "ECO-Identity",
                new ConventionPack() { new Conventions.ECOIdentityMapConvention() },
                type => type.GetProperty("Identity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly) != null
            );
        }
    }
}
