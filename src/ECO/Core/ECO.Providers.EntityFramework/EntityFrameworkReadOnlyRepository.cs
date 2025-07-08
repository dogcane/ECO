namespace ECO.Providers.EntityFramework;

using ECO.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class EntityFrameworkReadOnlyRepository<T, K>(IDataContext dataContext)
    : EntityFrameworkPersistenceManager<T, K>(dataContext), IReadOnlyRepository<T, K>
    where T : class, IAggregateRoot<K>
{
    #region IReadOnlyRepository<T,K> Members
    public virtual T? Load(K identity) => DbContext.Set<T>().Find(identity);
    public virtual ValueTask<T?> LoadAsync(K identity) => DbContext.Set<T>().FindAsync(identity);
    #endregion

    #region IEnumerable<T> Members
    public virtual IEnumerator<T> GetEnumerator() => DbContext.Set<T>().AsEnumerable().GetEnumerator();
    #endregion

    #region IEnumerable Members
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion

    #region IQueryable Members
    public virtual Type ElementType => DbContext.Set<T>().AsQueryable().ElementType;
    public virtual Expression Expression => DbContext.Set<T>().AsQueryable().Expression;
    public virtual IQueryProvider Provider => DbContext.Set<T>().AsQueryable().Provider;
    #endregion
}
