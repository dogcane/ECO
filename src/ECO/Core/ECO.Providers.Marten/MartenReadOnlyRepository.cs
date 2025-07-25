namespace ECO.Providers.Marten;

using ECO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class MartenReadOnlyRepository<T, K>(IDataContext dataContext) : MartenPersistenceManager<T, K>(dataContext), IReadOnlyRepository<T, K>
    where T : class, IAggregateRoot<K>
{
    #region IReadOnlyEntityManager<T,K> Members

    public virtual T? Load(K identity) => LoadAsync(identity).GetAwaiter().GetResult();

    public virtual ValueTask<T?> LoadAsync(K identity) => identity switch
    {
        string stringId => new ValueTask<T?>(DocumentSession.LoadAsync<T>(stringId)),
        int intId => new ValueTask<T?>(DocumentSession.LoadAsync<T>(intId)),
        long longId => new ValueTask<T?>(DocumentSession.LoadAsync<T>(longId)),
        Guid guidId => new ValueTask<T?>(DocumentSession.LoadAsync<T>(guidId)),
        _ => throw new InvalidOperationException($"Unsupported identity type: {typeof(K).Name}")
    };

    #endregion

    #region IEnumerable<T> Members

    public virtual IEnumerator<T> GetEnumerator() => DocumentSession.Query<T>().GetEnumerator();

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => DocumentSession.Query<T>().GetEnumerator();

    #endregion

    #region IQueryable Members

    public virtual Type ElementType => DocumentSession.Query<T>().ElementType;

    public virtual System.Linq.Expressions.Expression Expression => DocumentSession.Query<T>().Expression;

    public virtual IQueryProvider Provider => DocumentSession.Query<T>().Provider;

    #endregion
}
