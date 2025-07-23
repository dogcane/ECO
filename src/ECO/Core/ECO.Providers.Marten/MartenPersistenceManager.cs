namespace ECO.Providers.Marten;

using System;
using ECO.Data;
using global::Marten;

public abstract class MartenPersistenceManager<T, K>(IDataContext dataContext) : PersistenceManagerBase<T, K>(dataContext)
    where T : class, IAggregateRoot<K>
{
    #region Protected_Properties
    protected IDocumentSession DocumentSession => (PersistenceContext as MartenPersistenceContext ?? throw new InvalidCastException(nameof(DocumentSession))).Session;
    #endregion
}
