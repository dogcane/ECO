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
    public class MongoReadOnlyRepository<T, K> : MongoPersistenceManager<T, K>, IReadOnlyRepository<T, K>
        where T : IAggregateRoot<K>
    {
        #region IReadOnlyRepository<T,K> Members

        public T Load(K identity)
        {
            T entity = default(T);
            bool founded = false;
            if (((MongoPersistenceContext)GetCurrentContext()).IdentityMap.ContainsKey(identity))
            {
                entity = (T)((MongoPersistenceContext)GetCurrentContext()).IdentityMap[identity];
                founded = true;
            }
            if (!founded)
            {
                entity = GetCurrentCollection().FindOneAs<T>(Query.EQ("_id", BsonValue.Create(identity)));
            }
            return entity;
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
