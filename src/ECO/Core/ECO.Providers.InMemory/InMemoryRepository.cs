﻿using ECO.Data;
using System.Threading.Tasks;

namespace ECO.Providers.InMemory
{
    public class InMemoryRepository<T, K> : InMemoryReadOnlyRepository<T, K>, IRepository<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Ctor

        public InMemoryRepository(IDataContext dataContext)
            : base(dataContext)
        {

        }

        #endregion

        #region IRepository<T> Membri di

        public virtual void Add(T item)
        {
            if (!_IdentityMap.ContainsKey(item.Identity))
            {
                lock (_SycnLock)
                {
                    if (!_IdentityMap.ContainsKey(item.Identity))
                    {
                        _IdentityMap.Add(item.Identity, item);
                        _EntitySet.Add(item);
                    }
                }
            }
        }

        public virtual async Task AddAsync(T item) => await Task.Run(() => Add(item));

        public virtual void Update(T item)
        {
            if (_IdentityMap.ContainsKey(item.Identity))
            {
                lock (_SycnLock)
                {
                    if (_IdentityMap.ContainsKey(item.Identity))
                    {
                        _EntitySet[_EntitySet.IndexOf(item)] = item;
                    }
                }
            }
        }

        public virtual async Task UpdateAsync(T item) => await Task.Run(() => Update(item));

        public virtual void Remove(T item)
        {
            if (_IdentityMap.ContainsKey(item.Identity))
            {
                lock (_SycnLock)
                {
                    if (_IdentityMap.ContainsKey(item.Identity))
                    {
                        _IdentityMap.Remove(item.Identity);
                        _EntitySet.Remove(item);
                    }
                }
            }
        }

        public virtual async Task RemoveAsync(T item) => await Task.Run(() => Remove(item));

        #endregion
    }
}
