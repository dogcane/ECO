namespace ECO;

/// <summary>
/// Defines a contract for repositories that can expose their aggregate roots as an asynchronous stream.
/// </summary>
/// <typeparam name="T">The type of the aggregate root.</typeparam>
public interface IAsyncEnumerableSource<T>
{
    /// <summary>
    /// Streams the aggregate roots asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while enumerating.</param>
    /// <returns>An asynchronous stream of aggregate roots.</returns>
    IAsyncEnumerable<T> ToAsyncEnumerable(CancellationToken cancellationToken = default);
}
