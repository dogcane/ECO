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

    public virtual ValueTask<T?> LoadAsync(K identity) => typeof(K).Name switch
    {
        nameof(String) => new ValueTask<T?>(DocumentSession.LoadAsync<T>(Convert.ToString(identity)!)),
        nameof(Int32) => new ValueTask<T?>(DocumentSession.LoadAsync<T>(Convert.ToInt32(identity))),
        nameof(Int64) => new ValueTask<T?>(DocumentSession.LoadAsync<T>(Convert.ToInt64(identity))),
        nameof(Guid) => new ValueTask<T?>(DocumentSession.LoadAsync<T>(Guid.Parse(Convert.ToString(identity)!))),
        _ => throw new InvalidOperationException()
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
