namespace ECO.Data;

public class PersistenceUnitNotFoundException(string persistenceUnitName) : ApplicationException($"Persistence Unit '{persistenceUnitName}' not found")
{
    #region Public_Properties
    public string PersistenceUnitName { get; protected set; } = persistenceUnitName;
    #endregion
}
