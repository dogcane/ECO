using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Data
{
    public interface ITransactionContext : IDisposable
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

        Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task RollbackAsync(CancellationToken cancellationToken = default(CancellationToken));

        #endregion
    }
}