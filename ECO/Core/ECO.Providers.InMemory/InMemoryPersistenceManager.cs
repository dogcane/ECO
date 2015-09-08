using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECO;
using ECO.Data;

namespace ECO.Providers.InMemory
{
    public abstract class InMemoryPersistenceManager<T, K> : PersistenceManagerBase<T, K>
        where T : class, IAggregateRoot<K>
    {
        #region Protected_Methods

        protected IDictionary<K, T> GetIdentityMap()
        {
            InMemoryPersistenceContext context = GetCurrentContext() as InMemoryPersistenceContext;
            IDictionary<K, T> identityMap = context[string.Format("IDENTITYMAP_{0}", typeof(T).Name)] as IDictionary<K, T>;
            if (identityMap == null)
            {
                identityMap = new Dictionary<K, T>();
                context[string.Format("IDENTITYMAP_{0}", typeof(T).Name)] = identityMap;
            }
            return identityMap;
        }

        protected IList<T> GetEntitySet()
        {
            InMemoryPersistenceContext context = GetCurrentContext() as InMemoryPersistenceContext;
            IList<T> entitySet = context[string.Format("ENTITYSET_{0}", typeof(T).Name)] as IList<T>;
            if (entitySet == null)
            {
                entitySet = new List<T>();
                context[string.Format("ENTITYSET_{0}", typeof(T).Name)] = entitySet;
            }            
            return entitySet;
        }

        #endregion
    }
}
