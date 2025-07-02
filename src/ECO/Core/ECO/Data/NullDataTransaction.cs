using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Data
{
    public sealed class NullDataTransaction : IDataTransaction
    {
        #region Ctor

        public NullDataTransaction(IPersistenceContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region IDataTransaction Membri di

        public IPersistenceContext Context { get; private set; }

        public void Commit()
        {

        }

        public void Rollback()
        {

        }

        #endregion

        #region IDisposable Membri di

        public void Dispose()
        {

        }

        /// <summary>
        /// Asynchronously releases all resources used by the NullDataTransaction.
        /// Since this is a no-op transaction, no actual cleanup is performed.
        /// </summary>
        /// <returns>A completed ValueTask</returns>
        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default) => await Task.Run(() => Commit());

        public async Task RollbackAsync(CancellationToken cancellationToken = default) => await Task.Run(() => Rollback());

        #endregion
    }
}
