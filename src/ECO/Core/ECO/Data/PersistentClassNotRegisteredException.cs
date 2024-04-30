namespace ECO.Data;

public class PersistentClassNotRegisteredException(Type persistenClassType) : ApplicationException($"Persistent class '{persistenClassType?.Name}' not registered")
{
    #region Public_Properties
    public Type PersistentClassType { get; protected set; } = persistenClassType!;
    #endregion
}
