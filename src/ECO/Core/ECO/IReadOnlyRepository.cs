using ECO.Data;
using System.Threading.Tasks;

namespace ECO;

/// <summary>
/// Interface that defines a read-only repository for aggregate roots.
/// </summary>
/// <typeparam name="T">The type of the aggregate root.</typeparam>
/// <typeparam name="K">The type of the identifier.</typeparam>
public interface IReadOnlyRepository<T, K> : IQueryable<T>, IPersistenceManager<T, K>
    where T : class, IAggregateRoot<K>
{
    #region Methods

    /// <summary>
    /// Loads the entity from the repository by its identifier.
    /// </summary>
    /// <param name="identity">The identifier of the entity.</param>
    /// <returns>The entity if found; otherwise, <c>null</c>.</returns>
    T? Load(K identity);

    /// <summary>
    /// Asynchronously loads the entity from the repository by its identifier.
    /// </summary>
    /// <param name="identity">The identifier of the entity.</param>
    /// <returns>A ValueTask representing the asynchronous operation, with the entity if found; otherwise, <c>null</c>.</returns>
    ValueTask<T?> LoadAsync(K identity);

    #endregion
}
