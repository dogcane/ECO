namespace ECO.Providers.MongoDB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECO.Data;
using global::MongoDB.Driver;

public class MongoReadOnlyRepository<T, K> : MongoPersistenceManager<T, K>, IReadOnlyRepository<T, K>
    where T : class, IAggregateRoot<K>
    where K : notnull, IEquatable<K>
{
    #region Ctor

    public MongoReadOnlyRepository(string collectionName, IDataContext dataContext)
        : base(collectionName, dataContext) { }

    public MongoReadOnlyRepository(IDataContext dataContext)
        : base(dataContext) { }

    #endregion

    #region IReadOnlyRepository<T,K> Members

    public virtual T? Load(K identity)
    {
        if (identity is null) throw new ArgumentNullException(nameof(identity));
        if (IdentityMap.ContainsIdentity(identity))
        {
            return IdentityMap[identity] is T value ? value : null;
        }
        else
        {
            return Collection.Find(Builders<T>.Filter.Eq("Identity", identity)).FirstOrDefault();
        }
    }

    public virtual async ValueTask<T?> LoadAsync(K identity)
    {
        if (identity is null) throw new ArgumentNullException(nameof(identity));
        if (IdentityMap.ContainsIdentity(identity))
        {
            return IdentityMap[identity] is T value ? value : null;
        }
        else
        {
            return await Collection.Find(Builders<T>.Filter.Eq("Identity", identity)).FirstOrDefaultAsync();
        }
    }

    #endregion

    #region IEnumerable<T> Members

    public virtual IEnumerator<T> GetEnumerator() => Collection.AsQueryable().GetEnumerator();

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => Collection.AsQueryable().GetEnumerator();

    #endregion

    #region IQueryable Members

    public Type ElementType => Collection.AsQueryable().ElementType;

    public System.Linq.Expressions.Expression Expression => Collection.AsQueryable().Expression;

    public IQueryProvider Provider => Collection.AsQueryable().Provider;

    #endregion
}
