using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Data
{
    public interface ITransactionContext : IDisposable, IAsyncDisposable
    {
        #region Properties

        Guid TransactionContextId { get; }

        bool AutoCommit { get; }

        TransactionStatus Status { get; }

        IDataContext DataContext { get; }

        #endregion

        #region Methods

        void EnlistDataTransaction(IDataTransaction dataTransaction);

        void Commit();

        void Rollback();

        Task CommitAsync(CancellationToken cancellationToken = default);

        Task RollbackAsync(CancellationToken cancellationToken = default);

        #endregion
    }
}