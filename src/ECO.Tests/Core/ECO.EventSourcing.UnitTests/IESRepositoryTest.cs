using ECO.EventSourcing;
using ECO.Data;
using Xunit;

namespace ECO.EventSourcing.UnitTests;

public class IESRepositoryTest
{
    // Test implementation of IESRepository interface
    public class TestESAggregateRoot : ESAggregateRoot<int>
    {
        public string Name { get; private set; } = string.Empty;

        public TestESAggregateRoot() : base()
        {
        }

        public TestESAggregateRoot(int identity) : base(identity)
        {
        }
    }

    [Fact]
    public void Should_iesrepository_extend_ipersistencemanager()
    {
        // Act & Assert
        Assert.True(typeof(IPersistenceManager<TestESAggregateRoot, int>).IsAssignableFrom(typeof(IESRepository<TestESAggregateRoot, int>)));
    }

    [Fact]
    public void Should_have_load_method()
    {
        // Act
        var method = typeof(IESRepository<TestESAggregateRoot, int>).GetMethod("Load");

        // Assert
        Assert.NotNull(method);
        Assert.Equal(typeof(TestESAggregateRoot), method.ReturnType);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(int), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void Should_have_loadasync_method()
    {
        // Act
        var method = typeof(IESRepository<TestESAggregateRoot, int>).GetMethod("LoadAsync");

        // Assert
        Assert.NotNull(method);
        Assert.Equal(typeof(ValueTask<TestESAggregateRoot?>), method.ReturnType);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(int), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void Should_have_save_method()
    {
        // Act
        var method = typeof(IESRepository<TestESAggregateRoot, int>).GetMethod("Save");

        // Assert
        Assert.NotNull(method);
        Assert.Equal(typeof(void), method.ReturnType);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(TestESAggregateRoot), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void Should_have_saveasync_method()
    {
        // Act
        var method = typeof(IESRepository<TestESAggregateRoot, int>).GetMethod("SaveAsync");

        // Assert
        Assert.NotNull(method);
        Assert.Equal(typeof(Task), method.ReturnType);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(TestESAggregateRoot), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void Should_have_loadevents_method()
    {
        // Act
        var method = typeof(IESRepository<TestESAggregateRoot, int>).GetMethod("LoadEvents");

        // Assert
        Assert.NotNull(method);
        Assert.Equal(typeof(IEnumerable<dynamic>), method.ReturnType);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(int), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void Should_have_loadeventsasync_method()
    {
        // Act
        var method = typeof(IESRepository<TestESAggregateRoot, int>).GetMethod("LoadEventsAsync");

        // Assert
        Assert.NotNull(method);
        Assert.Equal(typeof(Task<IEnumerable<dynamic>>), method.ReturnType);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(int), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void Should_have_generic_constraints()
    {
        // Act
        var interfaceType = typeof(IESRepository<,>);
        var genericArguments = interfaceType.GetGenericArguments();

        // Assert
        Assert.Equal(2, genericArguments.Length);
        
        // T must be class and implement IESAggregateRoot<K>
        var tConstraints = genericArguments[0].GetGenericParameterConstraints();
        Assert.Contains(typeof(IESAggregateRoot<>).MakeGenericType(genericArguments[1]), tConstraints);
        
        // K must be notnull and implement IEquatable<K>
        var kConstraints = genericArguments[1].GetGenericParameterConstraints();
        Assert.Contains(typeof(IEquatable<>).MakeGenericType(genericArguments[1]), kConstraints);
    }
}