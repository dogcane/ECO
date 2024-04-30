namespace ECO.Data;

public interface IDataContext : IDisposable
{
    #region Proterties
    Guid DataContextId { get; }
    ITransactionContext? Transaction { get; }
    #endregion

    #region Methods
    void Close();
    void SaveChanges();
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    void Attach<T>(IAggregateRoot<T> entity);
    void Detach<T>(IAggregateRoot<T> entity);
    void Refresh<T>(IAggregateRoot<T> entity);
    ITransactionContext BeginTransaction();
    ITransactionContext BeginTransaction(bool autoCommit);
    Task<ITransactionContext> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<ITransactionContext> BeginTransactionAsync(bool autoCommit, CancellationToken cancellationToken = default);
    IPersistenceContext GetCurrentContext<T>();
    IPersistenceContext GetCurrentContext(object entity);
    IPersistenceContext GetCurrentContext(Type entityType);
    PersistenceState GetPersistenceState<T>(IAggregateRoot<T> entity);
    #endregion
}