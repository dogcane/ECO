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

        IDataTransaction? Transaction { get; }

        #endregion

        #region Methods

        void Attach<T>(IAggregateRoot<T> entity);

        void Detach<T>(IAggregateRoot<T> entity);

        void Refresh<T>(IAggregateRoot<T> entity);

        PersistenceState GetPersistenceState<T>(IAggregateRoot<T> entity);

        IDataTransaction BeginTransaction();

        Task<IDataTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        void Close();

        void SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        #endregion
    }
}
