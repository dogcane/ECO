using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Data
{
    public interface IPersistenceContext : IDisposable
    {
        #region Properties

        Guid PersistenceContextId { get; }

        IPersistenceUnit PersistenceUnit { get; }

        IDataTransaction Transaction { get; }

        #endregion

        #region Methods

        void Attach<T, K>(T entity) where T : IAggregateRoot<K>;

        void Detach<T, K>(T entity) where T : IAggregateRoot<K>;

        void Refresh<T, K>(T entity) where T : IAggregateRoot<K>;

        PersistenceState GetPersistenceState<T, K>(T entity) where T : IAggregateRoot<K>;

        IDataTransaction BeginTransaction();

        Task<IDataTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));

        void Close();

        void SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        #endregion
    }
}
