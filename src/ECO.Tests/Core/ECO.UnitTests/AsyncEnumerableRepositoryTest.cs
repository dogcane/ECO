using ECO.Data;
using ECO.Integrations.Moq;
using ECO.Providers.InMemory;
using ECO.UnitTests.Utils.Foos;
using Moq;
using Xunit;

namespace ECO.UnitTests;

public class AsyncEnumerableRepositoryTest
{
    private sealed class StreamingAggregateRoot(int identity) : AggregateRoot<int>(identity);

    private sealed class StreamingInMemoryRepository(IDataContext dataContext)
        : InMemoryRepository<StreamingAggregateRoot, int>(dataContext)
    {
        public static void Clear() => _EntitySet.Clear();
    }

    private static StreamingInMemoryRepository CreateRepository()
    {
        StreamingInMemoryRepository.Clear();
        return new StreamingInMemoryRepository(new Mock<IDataContext>().Object);
    }

    [Fact]
    public async Task Should_stream_all_entities_from_read_only_repository()
    {
        // Arrange
        var repository = CreateRepository();
        repository.Add(new StreamingAggregateRoot(1));
        repository.Add(new StreamingAggregateRoot(2));
        repository.Add(new StreamingAggregateRoot(3));
        IReadOnlyRepository<StreamingAggregateRoot, int> readOnlyRepository = repository;

        // Act
        var identities = new List<int>();
        await foreach (var entity in readOnlyRepository.ToAsyncEnumerable())
        {
            identities.Add(entity.Identity);
        }

        // Assert
        Assert.Equal(3, identities.Count);
        Assert.Equal(new[] { 1, 2, 3 }, identities.OrderBy(identity => identity));
    }

    [Fact]
    public async Task Should_honor_cancellation_during_async_enumeration()
    {
        // Arrange
        var repository = CreateRepository();
        repository.Add(new StreamingAggregateRoot(1));
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
            await foreach (var _ in repository.ToAsyncEnumerable(cancellationTokenSource.Token))
            {
            }
        });
    }

    [Fact]
    public async Task Should_snapshot_entities_when_enumeration_starts()
    {
        // Arrange
        var repository = CreateRepository();
        repository.Add(new StreamingAggregateRoot(1));
        repository.Add(new StreamingAggregateRoot(2));

        // Act
        var identities = new List<int>();
        await foreach (var entity in repository.ToAsyncEnumerable())
        {
            identities.Add(entity.Identity);
            if (identities.Count == 1)
            {
                repository.Add(new StreamingAggregateRoot(3));
            }
        }

        // Assert
        Assert.Equal(2, identities.Count);
    }

    [Fact]
    public async Task Should_stream_entities_from_mock_repository()
    {
        // Arrange
        var source = new List<AggregateRootFooOfInt> { new(1), new(2) };
        var repository = new Mock<IReadOnlyRepository<AggregateRootFooOfInt, int>>();
        repository.SetupReadOnlyRepository<IReadOnlyRepository<AggregateRootFooOfInt, int>, AggregateRootFooOfInt, int>(source);

        // Act
        var result = new List<AggregateRootFooOfInt>();
        await foreach (var entity in repository.Object.ToAsyncEnumerable())
        {
            result.Add(entity);
        }

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Should_fallback_to_synchronous_enumeration_when_async_source_is_not_available()
    {
        // Arrange
        var source = new List<AggregateRootFooOfInt> { new(1), new(2) };
        var repository = new Mock<IReadOnlyRepository<AggregateRootFooOfInt, int>>();
        repository.SetupQueryable(source);

        // Act
        var result = new List<AggregateRootFooOfInt>();
        await foreach (var entity in repository.Object.ToAsyncEnumerable())
        {
            result.Add(entity);
        }

        // Assert
        Assert.Equal(2, result.Count);
    }
}
