using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECO.Data
{
    public interface IDataContext : IDisposable
    {
        #region Proterties

        Guid DataContextId { get; }

        ITransactionContext Transaction { get; }

        #endregion

        #region Methods
        void Close();
        void SaveChanges();
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        void Attach<T, K>(T entity) where T : IAggregateRoot<K>;
        void Detach<T, K>(T entity) where T : IAggregateRoot<K>;
        void Refresh<T, K>(T entity) where T : IAggregateRoot<K>;
        ITransactionContext BeginTransaction();
        ITransactionContext BeginTransaction(bool autoCommit);
        Task<ITransactionContext> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task<ITransactionContext> BeginTransactionAsync(bool autoCommit, CancellationToken cancellationToken = default);
        IPersistenceContext GetCurrentContext<T>();
        IPersistenceContext GetCurrentContext(object entity);
        IPersistenceContext GetCurrentContext(Type entityType);
        PersistenceState GetPersistenceState<T, K>(T entity) where T : IAggregateRoot<K>;

        #endregion
    }
}