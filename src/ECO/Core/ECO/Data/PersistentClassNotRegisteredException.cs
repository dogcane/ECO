namespace ECO.Data;

/// <summary>
/// Exception thrown when a persistent class type is not registered in any persistence unit.
/// </summary>
public class PersistentClassNotRegisteredException(Type persistentClassType)
    : ApplicationException($"Persistent class '{persistentClassType?.Name}' not registered")
{
    #region Public_Properties

    /// <summary>
    /// Gets the persistent class type that was not registered.
    /// </summary>
    public Type PersistentClassType { get; } = persistentClassType ?? throw new ArgumentNullException(nameof(persistentClassType));

    #endregion
}
