namespace ECO.Data;

/// <summary>
/// Interface that represent a listener that subscribes the different events of a persistence context lifecycle
/// </summary>
public interface IPersistenceUnitListener
{
    #region Methods

    /// <summary>
    /// Method that occurs before the creation of the persistence context
    /// </summary>
    /// <param name="unit"></param>
    void ContextPreCreate(IPersistenceUnit unit);

    /// <summary>
    /// Method that occurs after the creation of the persistence context
    /// </summary>
    /// <param name="unit"></param>
    void ContextPostCreate(IPersistenceUnit unit, IPersistenceContext context);

    #endregion
}
