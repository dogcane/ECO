using System.Collections.Generic;

namespace ECO.Configuration
{
    /// <summary>
    /// ECO Options configuration.
    /// </summary>
    public class ECOOptions
    {
        public const string ECOConfigurationName = "eco";
        public PersistenceUnitOptions[] PersistenceUnits { get; set; } = new PersistenceUnitOptions[] { };
    }

    public class PersistenceUnitOptions
    {
        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string[] Listeners { get; set; } = new string[] { };

        public string[] Classes { get; set; } = new string[] { };

        public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
    }
}
