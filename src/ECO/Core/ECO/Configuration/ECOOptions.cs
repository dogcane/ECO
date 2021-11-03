using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.Configuration
{
    /// <summary>
    /// ECO Options configuration.
    /// A sample:
    ///
    ///	"eco": {
    ///			"persistenceUnits": {
    ///				"name" : "PersistentUnitName",
    ///				"type" : "PertistentUnitType",
    ///             "listeners": [
    ///					"ListenerType1",
    ///					"ListenerType2"
    ///				],
    ///				"classes": [
    ///					"ClassType1",
    ///					"ClassType2"
    ///				],
    ///				"attributes": [
    ///					{ 
    ///					    "key1": "PersistenteUnitAttributeName",
    ///						"value1": "PersistenteUnitAttributeValue"
    ///					},
    ///				    { 
    ///				        "key2": "PersistenteUnitAttributeName",
    ///						"value1": "PersistenteUnitAttributeValue"
    ///					},
    ///				]
    ///			}
    ///     }
    /// </summary>
    public class ECOOptions
    {
        public PersistenceUnitOptions[] PersistenceUnits { get; set; }
    }

    public class PersistenceUnitOptions
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string[] UnitListeners { get; set; }

        public string[] Classes { get; set; }

        public IDictionary<string, string> Attributes { get; set; }
    }
}
