namespace ECO.Data;

/// <summary>
/// Represents a listener that subscribes to events in the lifecycle of a persistence context.
/// </summary>
public interface IPersistenceUnitListener
{
    #region Methods

    /// <summary>
    /// Called before the creation of the persistence context.
    /// </summary>
    /// <param name="unit">The persistence unit for which the context is being created.</param>
    void ContextPreCreate(IPersistenceUnit unit);

    /// <summary>
    /// Called after the creation of the persistence context.
    /// </summary>
    /// <param name="unit">The persistence unit for which the context was created.</param>
    /// <param name="context">The created persistence context.</param>
    void ContextPostCreate(IPersistenceUnit unit, IPersistenceContext context);

    #endregion
}
