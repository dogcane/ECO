using System;

namespace ECO.Data
{
    public interface ITransactionContext : IDisposable
    {
        #region Properties

        bool AutoCommit { get; }
        TransactionStatus Status { get; }
        #endregion
        #region Methods
        void EnlistDataTransaction(IDataTransaction transaction);
        void Commit();
        void Rollback();
        #endregion
    }
}