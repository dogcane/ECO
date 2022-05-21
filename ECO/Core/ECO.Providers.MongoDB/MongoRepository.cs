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
        where T : class, IAggregateRoot<K>
    {
        #region IRepository<T,K> Members

        public void Add(T item)
        {
            GetCurrentCollection().Save(item);
        }

        public async Task AddAsync(T item)
        {
            await Task.Run(() => Add(item));
        }

        public void Update(T item)
        {
            GetCurrentCollection().Save(item);
        }

        public async Task UpdateAsync(T item)
        {
            await Task.Run(() => Update(item));
        }

        public void Remove(T item)
        {
            GetCurrentCollection().Remove(Query<T>.EQ(e => e.Identity, item.Identity));
        }

        public async Task RemoveAsync(T item)
        {
            await Task.Run(() => Remove(item));
        }

        #endregion
    }
}
