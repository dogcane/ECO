using ECO.Data;
using ECO.UnitTests.Utils.Foos;
using Xunit;

namespace ECO.UnitTests;

public class IRepositoryTest
{
    [Fact]
    public void Should_ireadonly_repository_extend_iqueryable_and_ipersistencemanager()
    {
        // Act & Assert
        Assert.True(typeof(IQueryable<AggregateRootFooOfInt>).IsAssignableFrom(typeof(IReadOnlyRepository<AggregateRootFooOfInt, int>)));
        Assert.True(typeof(IPersistenceManager<AggregateRootFooOfInt, int>).IsAssignableFrom(typeof(IReadOnlyRepository<AggregateRootFooOfInt, int>)));
    }

    [Fact]
    public void Should_irepository_extend_ireadonly_repository()
    {
        // Act & Assert
        Assert.True(typeof(IReadOnlyRepository<AggregateRootFooOfInt, int>).IsAssignableFrom(typeof(IRepository<AggregateRootFooOfInt, int>)));
    }

    [Fact]
    public void Should_ireadonly_repository_have_load_method()
    {
        // Act
        var method = typeof(IReadOnlyRepository<AggregateRootFooOfInt, int>).GetMethod("Load");

        // Assert
        Assert.NotNull(method);
        Assert.Equal(typeof(AggregateRootFooOfInt), method.ReturnType);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(int), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void Should_ireadonly_repository_have_loadasync_method()
    {
        // Act
        var method = typeof(IReadOnlyRepository<AggregateRootFooOfInt, int>).GetMethod("LoadAsync");

        // Assert
        Assert.NotNull(method);
        Assert.Equal(typeof(ValueTask<AggregateRootFooOfInt?>), method.ReturnType);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(int), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void Should_irepository_have_add_method()
    {
        // Act
        var method = typeof(IRepository<AggregateRootFooOfInt, int>).GetMethod("Add");

        // Assert
        Assert.NotNull(method);
        Assert.Equal(typeof(void), method.ReturnType);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(AggregateRootFooOfInt), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void Should_irepository_have_addasync_method()
    {
        // Act
        var method = typeof(IRepository<AggregateRootFooOfInt, int>).GetMethod("AddAsync");

        // Assert
        Assert.NotNull(method);
        Assert.Equal(typeof(Task), method.ReturnType);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(AggregateRootFooOfInt), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void Should_irepository_have_update_method()
    {
        // Act
        var method = typeof(IRepository<AggregateRootFooOfInt, int>).GetMethod("Update");

        // Assert
        Assert.NotNull(method);
        Assert.Equal(typeof(void), method.ReturnType);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(AggregateRootFooOfInt), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void Should_irepository_have_updateasync_method()
    {
        // Act
        var method = typeof(IRepository<AggregateRootFooOfInt, int>).GetMethod("UpdateAsync");

        // Assert
        Assert.NotNull(method);
        Assert.Equal(typeof(Task), method.ReturnType);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(AggregateRootFooOfInt), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void Should_irepository_have_remove_method()
    {
        // Act
        var method = typeof(IRepository<AggregateRootFooOfInt, int>).GetMethod("Remove");

        // Assert
        Assert.NotNull(method);
        Assert.Equal(typeof(void), method.ReturnType);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(AggregateRootFooOfInt), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void Should_irepository_have_removeasync_method()
    {
        // Act
        var method = typeof(IRepository<AggregateRootFooOfInt, int>).GetMethod("RemoveAsync");

        // Assert
        Assert.NotNull(method);
        Assert.Equal(typeof(Task), method.ReturnType);
        Assert.Single(method.GetParameters());
        Assert.Equal(typeof(AggregateRootFooOfInt), method.GetParameters()[0].ParameterType);
    }

    [Fact]
    public void Should_repository_interfaces_have_generic_constraints()
    {
        // Act
        var readOnlyRepoType = typeof(IReadOnlyRepository<,>);
        var repoType = typeof(IRepository<,>);
        var readOnlyGenericArgs = readOnlyRepoType.GetGenericArguments();
        var repoGenericArgs = repoType.GetGenericArguments();

        // Assert
        Assert.Equal(2, readOnlyGenericArgs.Length);
        Assert.Equal(2, repoGenericArgs.Length);
        
        // T must be class and implement IAggregateRoot<K>
        var tConstraints = readOnlyGenericArgs[0].GetGenericParameterConstraints();
        Assert.Contains(typeof(IAggregateRoot<>).MakeGenericType(readOnlyGenericArgs[1]), tConstraints);
        
        // K must be notnull and implement IEquatable<K>
        var kConstraints = readOnlyGenericArgs[1].GetGenericParameterConstraints();
        Assert.Contains(typeof(IEquatable<>).MakeGenericType(readOnlyGenericArgs[1]), kConstraints);
    }
}