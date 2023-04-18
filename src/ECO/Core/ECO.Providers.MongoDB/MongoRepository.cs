﻿using ECO.Data;
using MongoDB.Driver;
using System;
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

        public void Add(T item) => Collection.InsertOne(item ?? throw new ArgumentNullException(nameof(item)));

        public async Task AddAsync(T item) => await Collection.InsertOneAsync(item ?? throw new ArgumentNullException(nameof(item)));

        public void Update(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            Collection.ReplaceOne(Builders<T>.Filter.Eq("Identity", item.Identity), item);
        }

        public async Task UpdateAsync(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            await Collection.ReplaceOneAsync(Builders<T>.Filter.Eq("Identity", item.Identity), item);
        }

        public void Remove(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            Collection.DeleteOne(Builders<T>.Filter.Eq("Identity", item.Identity));
        }

        public async Task RemoveAsync(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            await Collection.DeleteOneAsync(Builders<T>.Filter.Eq("Identity", item.Identity));
        }

        #endregion
    }
}
