namespace ECO.Utils;

/// <summary>
/// Extension methods for working with aggregate root types.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Determines whether the specified type is a non-abstract class implementing <see cref="IAggregateRoot{T}"/>.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the type is a concrete aggregate root; otherwise, <c>false</c>.</returns>
    public static bool IsAggregateRootType(this Type type) =>
        type.IsClass && !type.IsAbstract && type.GetInterface(typeof(IAggregateRoot<>).Name) != null;

    /// <summary>
    /// Filters a sequence of types, returning only those that are aggregate root types.
    /// </summary>
    /// <param name="types">The sequence of types to filter.</param>
    /// <returns>An <see cref="IEnumerable{Type}"/> of aggregate root types.</returns>
    public static IEnumerable<Type> OfAggregateRootType(this IEnumerable<Type> types) =>
        types.Where(t => t.IsAggregateRootType());
}
