namespace ECO;

/// <summary>
/// Interface that defines a repository for aggregate roots, supporting CRUD operations.
/// </summary>
/// <typeparam name="T">The type of the aggregate root.</typeparam>
/// <typeparam name="K">The type of the identifier.</typeparam>
public interface IRepository<T, K> : IReadOnlyRepository<T, K>
    where T : class, IAggregateRoot<K>
{
    #region Methods

    /// <summary>
    /// Adds the entity to the repository.
    /// </summary>
    /// <param name="item">The entity to add.</param>
    void Add(T item);

    /// <summary>
    /// Asynchronously adds the entity to the repository.
    /// </summary>
    /// <param name="item">The entity to add.</param>
    Task AddAsync(T item);

    /// <summary>
    /// Updates the entity in the repository.
    /// </summary>
    /// <param name="item">The entity to update.</param>
    void Update(T item);

    /// <summary>
    /// Asynchronously updates the entity in the repository.
    /// </summary>
    /// <param name="item">The entity to update.</param>
    Task UpdateAsync(T item);

    /// <summary>
    /// Removes the entity from the repository.
    /// </summary>
    /// <param name="item">The entity to remove.</param>
    void Remove(T item);

    /// <summary>
    /// Asynchronously removes the entity from the repository.
    /// </summary>
    /// <param name="item">The entity to remove.</param>
    Task RemoveAsync(T item);

    #endregion
}
