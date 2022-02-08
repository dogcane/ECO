using ECO.Data;
using System.Threading.Tasks;

namespace ECO
{
    /// <summary>
    /// Interface that defines a repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public interface IRepository<T, K> : IReadOnlyRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Methods

        /// <summary>
        /// Method that adds asynchronously the entity to the repository
        /// </summary>
        /// <param name="item"></param>
        void Add(T item);

        /// <summary>
        /// Method that adds the entity to the repository
        /// </summary>
        /// <param name="item"></param>
        Task AddAsync(T item);

        /// <summary>
        /// Method that update the entity in the repository
        /// </summary>
        /// <param name="item"></param>
        void Update(T item);

        /// <summary>
        /// Method that update asynchronously the entity in the repository
        /// </summary>
        /// <param name="item"></param>
        Task UpdateAsync(T item);

        /// <summary>
        /// Method that removes the entity from the repository
        /// </summary>
        /// <param name="item"></param>
        void Remove(T item);

        /// <summary>
        /// Method that removes asynchronously the entity from the repository
        /// </summary>
        /// <param name="item"></param>
        Task RemoveAsync(T item);

        #endregion
    }
}
