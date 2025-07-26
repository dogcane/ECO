using ECO.Utils;
using ECO.UnitTests.Utils.Foos;
using Xunit;

namespace ECO.UnitTests.Utils;

public class TypeExtensionsTest
{
    [Fact]
    public void Should_return_true_for_aggregate_root_type()
    {
        // Arrange
        var aggregateType = typeof(AggregateRootFooOfInt);

        // Act
        var result = aggregateType.IsAggregateRootType();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Should_return_true_for_versionable_aggregate_root_type()
    {
        // Arrange
        var aggregateType = typeof(VersionableAggregateRootFooOfInt);

        // Act
        var result = aggregateType.IsAggregateRootType();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Should_return_false_for_entity_type()
    {
        // Arrange
        var entityType = typeof(EntityFooOfInt);

        // Act
        var result = entityType.IsAggregateRootType();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Should_return_false_for_abstract_aggregate_type()
    {
        // Arrange
        var abstractType = typeof(AggregateRoot<int>);

        // Act
        var result = abstractType.IsAggregateRootType();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Should_return_false_for_interface_type()
    {
        // Arrange
        var interfaceType = typeof(IAggregateRoot<int>);

        // Act
        var result = interfaceType.IsAggregateRootType();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Should_return_false_for_non_aggregate_class()
    {
        // Arrange
        var stringType = typeof(string);

        // Act
        var result = stringType.IsAggregateRootType();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Should_filter_aggregate_root_types_from_collection()
    {
        // Arrange
        var types = new[]
        {
            typeof(AggregateRootFooOfInt),
            typeof(VersionableAggregateRootFooOfInt),
            typeof(EntityFooOfInt),
            typeof(string),
            typeof(IAggregateRoot<int>),
            typeof(AggregateRoot<int>)
        };

        // Act
        var aggregateTypes = types.OfAggregateRootType().ToList();

        // Assert
        Assert.Equal(2, aggregateTypes.Count);
        Assert.Contains(typeof(AggregateRootFooOfInt), aggregateTypes);
        Assert.Contains(typeof(VersionableAggregateRootFooOfInt), aggregateTypes);
    }

    [Fact]
    public void Should_return_empty_when_no_aggregate_types_in_collection()
    {
        // Arrange
        var types = new[]
        {
            typeof(EntityFooOfInt),
            typeof(string),
            typeof(int),
            typeof(IAggregateRoot<int>)
        };

        // Act
        var aggregateTypes = types.OfAggregateRootType().ToList();

        // Assert
        Assert.Empty(aggregateTypes);
    }
}