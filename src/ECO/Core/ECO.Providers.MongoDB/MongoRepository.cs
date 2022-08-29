using ECO.Data;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ECO.Providers.MongoDB
{
    public class MongoRepository<T, K> : MongoReadOnlyRepository<T, K>, IRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Ctor

        public MongoRepository(string collectionName, IDataContext dataContext)
            : base(collectionName, dataContext)
        {

        }

        public MongoRepository(IDataContext dataContext)
            : base(dataContext)
        {

        }

        #endregion

        #region IRepository<T,K> Members

        public void Add(T item) => Collection.InsertOne(item);

        public async Task AddAsync(T item) => await Collection.InsertOneAsync(item);

        public void Update(T item) => Collection.ReplaceOne(Builders<T>.Filter.Eq("Identity", item.Identity), item);

        public async Task UpdateAsync(T item) => await Collection.ReplaceOneAsync(Builders<T>.Filter.Eq("Identity", item.Identity), item);

        public void Remove(T item) => Collection.DeleteOne(Builders<T>.Filter.Eq("Identity", item.Identity));

        public async Task RemoveAsync(T item) => await Collection.DeleteOneAsync(Builders<T>.Filter.Eq("Identity", item.Identity));

        #endregion
    }
}
