using Xunit;

namespace ECO.UnitTests;

public class PersistenceStateTest
{
    [Fact]
    public void Should_have_unknown_state_with_value_zero()
    {
        // Act & Assert
        Assert.Equal(0, (int)PersistenceState.Unknown);
    }

    [Fact]
    public void Should_have_transient_state_with_value_one()
    {
        // Act & Assert
        Assert.Equal(1, (int)PersistenceState.Transient);
    }

    [Fact]
    public void Should_have_persistent_state_with_value_two()
    {
        // Act & Assert
        Assert.Equal(2, (int)PersistenceState.Persistent);
    }

    [Fact]
    public void Should_have_detached_state_with_value_three()
    {
        // Act & Assert
        Assert.Equal(3, (int)PersistenceState.Detached);
    }

    [Fact]
    public void Should_be_able_to_compare_states()
    {
        // Arrange
        var unknown = PersistenceState.Unknown;
        var transient = PersistenceState.Transient;
        var persistent = PersistenceState.Persistent;
        var detached = PersistenceState.Detached;

        // Act & Assert
        Assert.True(unknown == PersistenceState.Unknown);
        Assert.True(transient == PersistenceState.Transient);
        Assert.True(persistent == PersistenceState.Persistent);
        Assert.True(detached == PersistenceState.Detached);
        Assert.False(unknown == transient);
    }

    [Fact]
    public void Should_support_enum_conversion()
    {
        // Act & Assert
        Assert.Equal(PersistenceState.Unknown, (PersistenceState)0);
        Assert.Equal(PersistenceState.Transient, (PersistenceState)1);
        Assert.Equal(PersistenceState.Persistent, (PersistenceState)2);
        Assert.Equal(PersistenceState.Detached, (PersistenceState)3);
    }
}