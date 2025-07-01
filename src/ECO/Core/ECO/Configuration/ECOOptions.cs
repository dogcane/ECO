namespace ECO.Configuration;

/// <summary>
/// ECO options configuration for persistence units.
/// </summary>
public class ECOOptions
{
    /// <summary>
    /// The configuration section name for ECO options.
    /// </summary>
    public const string ECOConfigurationName = "eco";

    /// <summary>
    /// The collection of persistence unit options.
    /// </summary>
    public PersistenceUnitOptions[] PersistenceUnits { get; set; } = [];
}

/// <summary>
/// Options for configuring a persistence unit.
/// </summary>
public class PersistenceUnitOptions
{
    /// <summary>
    /// The name of the persistence unit.
    /// </summary>
    public required string Name { get; set; } = string.Empty;

    /// <summary>
    /// The type name of the persistence unit implementation.
    /// </summary>
    public required string Type { get; set; } = string.Empty;

    /// <summary>
    /// The list of listener type names to attach to the persistence unit.
    /// </summary>
    public string[] Listeners { get; set; } = [];

    /// <summary>
    /// The list of class type names managed by the persistence unit.
    /// </summary>
    public string[] Classes { get; set; } = [];

    /// <summary>
    /// Additional attributes for the persistence unit.
    /// </summary>
    public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
}
