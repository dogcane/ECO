using System;

namespace ECO.Data
{
    /// <summary>
    /// Interface that defines a data transaction.
    /// </summary>
    public interface IDataTransaction : IDisposable
    {
        #region Properties

        /// <summary>
        /// Corrent context, owner of the data transaction
        /// </summary>
        IPersistenceContext Context { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Method that commits the transaction
        /// </summary>
        void Commit();

        /// <summary>
        /// Method that rollbacks the transaction
        /// </summary>
        void Rollback();

        #endregion
    }
}
