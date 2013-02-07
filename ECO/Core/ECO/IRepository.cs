using System;
using System.Collections.Generic;

using ECO.Data;

namespace ECO
{
    /// <summary>
    /// Interface that defines a repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public interface IRepository<T, K> : IReadOnlyRepository<T, K>, IPersistenceManager<T, K>
        where T : IAggregateRoot<K>
    {
        #region Methods

        /// <summary>
        /// Method that adds the entity to the repository
        /// </summary>
        /// <param name="item"></param>
        void Add(T item);

        /// <summary>
        /// Method that removes the entity from the repository
        /// </summary>
        /// <param name="item"></param>
        void Remove(T item);

        #endregion
    }
}
