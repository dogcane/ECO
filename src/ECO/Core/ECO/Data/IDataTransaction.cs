using System;
using System.Threading;
using System.Threading.Tasks;

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

        /// <summary>
        /// Method that commits the transaction asynchronously
        /// </summary>
        Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Method that rollbacks the transaction asynchronously
        /// </summary>
        Task RollbackAsync(CancellationToken cancellationToken = default(CancellationToken));

        #endregion
    }
}
