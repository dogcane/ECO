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

        void Initialize(string name, IDictionary<string, string> extendedAttributes);

        IPersistenceContext GetCurrentContext();

        void AddClass(Type classType);

        void RemoveClass(Type classType);

        void AddUnitListener(IPersistenceUnitListener listener);

        void RemoveUnitListener(IPersistenceUnitListener listener);

        #endregion
    }
}
