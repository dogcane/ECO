using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.Configuration
{
    /// <summary>
    /// ECO Options configuration.
    /// </summary>
    public class ECOOptions
    {
        public const string ECOConfigurationName = "eco";
        public PersistenceUnitOptions[] PersistenceUnits { get; set; }
    }

    public class PersistenceUnitOptions
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string[] Listeners { get; set; }

        public string[] Classes { get; set; }

        public IDictionary<string, string> Attributes { get; set; }
    }
}
