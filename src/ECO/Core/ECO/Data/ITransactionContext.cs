using System;

namespace ECO.Data
{
    public interface ITransactionContext : IDisposable
    {
        #region Properties

        Guid TransactionContextId { get; }

        bool AutoCommit { get; }

        TransactionStatus Status { get; }

        #endregion

        #region Methods

        void EnlistDataTransaction(IDataTransaction dataTransaction);

        void Commit();

        void Rollback();

        #endregion
    }
}