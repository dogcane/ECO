namespace ECO;

/// <summary>
/// Provides asynchronous streaming extensions for read-only repositories.
/// </summary>
public static class ReadOnlyRepositoryExtensions
{
    /// <summary>
    /// Streams the aggregate roots of the repository asynchronously.
    /// </summary>
    /// <remarks>
    /// When the repository implements <see cref="IAsyncEnumerableSource{T}"/>, the provider-native streaming implementation is used.
    /// Otherwise, the underlying <see cref="IQueryable{T}"/> is enumerated synchronously inside an asynchronous iterator.
    /// </remarks>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <typeparam name="K">The type of the aggregate root's identifier.</typeparam>
    /// <param name="repository">The repository to stream.</param>
    /// <param name="cancellationToken">A cancellation token to observe while enumerating.</param>
    /// <returns>An asynchronous stream of aggregate roots.</returns>
    public static IAsyncEnumerable<T> ToAsyncEnumerable<T, K>(
        this IReadOnlyRepository<T, K> repository,
        CancellationToken cancellationToken = default)
        where T : class, IAggregateRoot<K>
        where K : notnull, IEquatable<K>
    {
        ArgumentNullException.ThrowIfNull(repository);

        return repository is IAsyncEnumerableSource<T> source
            ? source.ToAsyncEnumerable(cancellationToken)
            : AsyncEnumerableFallback.ToAsyncEnumerable(repository, cancellationToken);
    }
}
