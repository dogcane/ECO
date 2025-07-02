using ECO.Configuration;
using Xunit;

namespace ECO.UnitTests.Configuration;

public class ECOOptionsTests
{
    [Fact]
    public void Should_defaultconstructor_initializes_empty_PersistenceUnits()
    {
        var options = new ECOOptions();
        Assert.NotNull(options.PersistenceUnits);
        Assert.Empty(options.PersistenceUnits);
    }

    [Fact]
    public void Should_ECOConfigurationName_have_correct_calue()
    {
        Assert.Equal("eco", ECOOptions.ECOConfigurationName);
    }

    [Fact]
    public void PersistenceUnitOptions_PropertiesInitializeCorrectly()
    {
        // Arrange
        var puOptions = new PersistenceUnitOptions
        {
            Name = "TestUnit",
            Type = "SQL"
        };

        // Act & Assert
        Assert.Equal("TestUnit", puOptions.Name);
        Assert.Equal("SQL", puOptions.Type);
        Assert.NotNull(puOptions.Listeners);
        Assert.Empty(puOptions.Listeners);
        Assert.NotNull(puOptions.Classes);
        Assert.Empty(puOptions.Classes);
        Assert.NotNull(puOptions.Attributes);
        Assert.Empty(puOptions.Attributes);
    }

    [Fact]
    public void ECOOptions_CanAddPersistenceUnit()
    {
        // Arrange
        var options = new ECOOptions();
        var puOptions = new PersistenceUnitOptions
        {
            Name = "TestUnit",
            Type = "SQL",
            Listeners = new[] { "Listener1", "Listener2" },
            Classes = new[] { "Class1", "Class2" },
            Attributes = new Dictionary<string, string>
            {
                ["ConnectionString"] = "Server=localhost;Database=TestDB",
                ["Timeout"] = "30"
            }
        };

        // Act
        options.PersistenceUnits = new[] { puOptions };

        // Assert
        Assert.Single(options.PersistenceUnits);
        var persistenceUnit = options.PersistenceUnits[0];
        Assert.Equal("TestUnit", persistenceUnit.Name);
        Assert.Equal("SQL", persistenceUnit.Type);
        Assert.Equal(2, persistenceUnit.Listeners.Length);
        Assert.Equal(2, persistenceUnit.Classes.Length);
        Assert.Equal(2, persistenceUnit.Attributes.Count);
        Assert.Equal("Server=localhost;Database=TestDB", persistenceUnit.Attributes["ConnectionString"]);
        Assert.Equal("30", persistenceUnit.Attributes["Timeout"]);
    }
}
