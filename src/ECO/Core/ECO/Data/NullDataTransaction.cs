using System.Threading;
using System.Threading.Tasks;

namespace ECO.Data
{
    public sealed class NullDataTransaction : IDataTransaction
    {
        #region Ctor

        public NullDataTransaction(IPersistenceContext context) => Context = context;

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

        public async Task CommitAsync(CancellationToken cancellationToken = default) => await Task.Run(() => Commit());        

        public async Task RollbackAsync(CancellationToken cancellationToken = default) => await Task.Run(() => Rollback());

        #endregion
    }
}
