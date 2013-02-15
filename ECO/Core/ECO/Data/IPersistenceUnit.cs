using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.Data
{
    /// <summary>
    /// Interface that defines a persistence unit. A persistent unit is made of the final storage
    /// (a relationa database for example) and of all the classes that are persisted in this storage.
    /// </summary>
    public interface IPersistenceUnit
    {
        #region Properties

        /// <summary>
        /// Name of the persistence unit
        /// </summary>
        string Name { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Method that initialize the persistence unit
        /// </summary>
        /// <param name="name"></param>
        /// <param name="extendedAttributes"></param>
        void Initialize(string name, IDictionary<string, string> extendedAttributes);

        /// <summary>
        /// Method that return the current persistence context for the opened data context (if there is one).
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Throw an ApplicationException if there is no active data context</exception>
        IPersistenceContext GetCurrentContext();

        /// <summary>
        /// Add a class type to the current persistence unit
        /// </summary>
        /// <param name="classType"></param>
        void AddClass(Type classType);

        /// <summary>
        /// Remove a class type from the current persistence unit
        /// </summary>
        /// <param name="classType"></param>
        void RemoveClass(Type classType);

        /// <summary>
        /// Add a listener to the current persistence unit
        /// </summary>
        /// <param name="listener"></param>
        void AddUnitListener(IPersistenceUnitListener listener);

        /// <summary>
        /// Remove a listener from the current persistence unit
        /// </summary>
        /// <param name="listener"></param>
        void RemoveUnitListener(IPersistenceUnitListener listener);

        #endregion
    }
}
