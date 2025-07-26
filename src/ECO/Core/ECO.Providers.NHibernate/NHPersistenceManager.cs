namespace ECO.Providers.NHibernate;

using System;
using ECO.Data;
using global::NHibernate;

public class NHPersistenceManager<T, K>(IDataContext dataContext) : PersistenceManagerBase<T, K>(dataContext)
    where T : class, IAggregateRoot<K>
    where K : notnull, IEquatable<K>
{
    #region Protected_Methods

    protected ISession GetCurrentSession()
        => (PersistenceContext as NHPersistenceContext ?? throw new InvalidCastException(nameof(NHPersistenceContext))).Session;

    protected ITransaction? GetCurrentTransaction()
        => SessionExtensions.GetCurrentTransaction(GetCurrentSession());

    #endregion
}
