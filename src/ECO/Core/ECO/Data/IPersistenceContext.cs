using System;

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

        void Close();

        void SaveChanges();

        #endregion
    }
}
