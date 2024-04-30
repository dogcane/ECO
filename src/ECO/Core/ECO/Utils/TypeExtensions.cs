namespace ECO.Utils;

public static class TypeExtensions
{
    public static bool IsAggregateRootType(this Type type) => type.IsClass && !type.IsAbstract && type.GetInterface(typeof(IAggregateRoot<>).Name) != null;

    public static IEnumerable<Type> OfAggregateRootType(this IEnumerable<Type> types) => types.Where(t => t.IsAggregateRootType());

}
