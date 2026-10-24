using System.Runtime.CompilerServices;

namespace ECO;

/// <summary>
/// Provides a fallback asynchronous enumeration for repositories that do not expose a native asynchronous stream.
/// </summary>
internal static class AsyncEnumerableFallback
{
    /// <summary>
    /// Enumerates the source synchronously inside an asynchronous iterator, observing the cancellation token.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    /// <param name="source">The source to enumerate.</param>
    /// <param name="cancellationToken">A cancellation token to observe while enumerating.</param>
    /// <returns>An asynchronous stream over the source.</returns>
    internal static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(
        IEnumerable<T> source,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);

        foreach (var item in source)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return item;
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }
}
