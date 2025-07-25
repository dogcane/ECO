namespace ECO.Providers.MongoDB.Conventions;

using global::MongoDB.Bson.Serialization;
using global::MongoDB.Bson.Serialization.Conventions;
using System.Reflection;

public class ECOIdentityMapConvention : IClassMapConvention
{
    public string Name => "ECOMapConvention";

    public void Apply(BsonClassMap classMap) => classMap?.MapIdProperty("Identity");

    public static void Register() => ConventionRegistry.Register(
            "ECO-Identity",
            new ConventionPack() { new ECOIdentityMapConvention() },
            type => type.GetProperty("Identity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly) != null
        );
}
