using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace ECO.Providers.MongoDB
{
    public abstract class MongoReadOnlyRepository<T, K> : MongoPersistenceManager<T, K>, IReadOnlyRepository<T, K>
        where T : AggregateRoot<K>
    {
        #region IReadOnlyRepository<T,K> Members

        public T Load(K identity)
        {
            return GetCurrentCollection().FindOneAs<T>(Query<T>.EQ(e => e.Identity, identity));
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return GetCurrentCollection().AsQueryable<T>().GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetCurrentCollection().AsQueryable<T>().GetEnumerator();
        }

        #endregion

        #region IQueryable Members

        public Type ElementType
        {
            get { return GetCurrentCollection().AsQueryable<T>().ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return GetCurrentCollection().AsQueryable<T>().Expression; }
        }

        public IQueryProvider Provider
        {
            get { return GetCurrentCollection().AsQueryable<T>().Provider; }
        }

        #endregion
    }
}
