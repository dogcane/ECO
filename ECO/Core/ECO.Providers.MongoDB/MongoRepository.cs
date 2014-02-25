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
    public class MongoRepository<T, K> : MongoReadOnlyRepository<T, K>, IRepository<T, K>
        where T : IAggregateRoot<K>
    {
        #region IRepository<T,K> Members

        public void Add(T item)
        {
            GetCurrentCollection().Save(item);
        }

        public void Update(T item)
        {
            GetCurrentCollection().Save(item);
        }

        public void Remove(T item)
        {
            GetCurrentCollection().Remove(Query<T>.EQ(e => e.Identity, item.Identity));
        }

        #endregion
    }
}
