namespace ECO.Data;

/// <summary>
/// Exception thrown when a requested persistence unit cannot be found.
/// </summary>
public class PersistenceUnitNotFoundException(string persistenceUnitName)
    : ApplicationException($"Persistence Unit '{persistenceUnitName}' not found")
{
    #region Public_Properties

    /// <summary>
    /// Gets the name of the persistence unit that was not found.
    /// </summary>
    public string PersistenceUnitName { get; } = persistenceUnitName ?? throw new ArgumentNullException(nameof(persistenceUnitName));

    #endregion
}
