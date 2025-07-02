namespace ECO;

/// <summary>
/// Interface that defines a value object.
/// </summary>
/// <typeparam name="T">The type of the value object.</typeparam>
public interface IValueObject<T> : IEquatable<T>
{
    // Marker interface for value objects.
}
