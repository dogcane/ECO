using System.Linq;

using ECO.Data;

namespace ECO
{
    /// <summary>
    /// Interface that defines a read only repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public interface IReadOnlyRepository<T, K> : IQueryable<T>, IPersistenceManager<T, K>
        where T : IAggregateRoot<K>
    {
        #region

        /// <summary>
        /// Method that loads the entity from the repository
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        T Load(K identity);

        #endregion
    }
}