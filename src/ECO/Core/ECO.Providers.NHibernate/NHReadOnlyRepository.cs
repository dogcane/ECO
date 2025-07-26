namespace ECO.Providers.NHibernate;

using ECO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class NHReadOnlyRepository<T, K>(IDataContext dataContext) : NHPersistenceManager<T, K>(dataContext), IReadOnlyRepository<T, K>
    where T : class, IAggregateRoot<K>
    where K : notnull, IEquatable<K>
{
    #region IReadOnlyEntityManager<T,K> Members

    public virtual T? Load(K identity) => GetCurrentSession().Get<T>(identity);

    public virtual async ValueTask<T?> LoadAsync(K identity) => await GetCurrentSession().GetAsync<T>(identity);

    #endregion

    #region IEnumerable<T> Members

    public virtual IEnumerator<T> GetEnumerator() => GetCurrentSession().Query<T>().GetEnumerator();

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetCurrentSession().Query<T>().GetEnumerator();

    #endregion

    #region IQueryable Members

    public virtual Type ElementType => GetCurrentSession().Query<T>().ElementType;

    public virtual System.Linq.Expressions.Expression Expression => GetCurrentSession().Query<T>().Expression;

    public virtual IQueryProvider Provider => GetCurrentSession().Query<T>().Provider;

    #endregion
}
