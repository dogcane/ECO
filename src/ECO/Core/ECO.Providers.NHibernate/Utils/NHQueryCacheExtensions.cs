using NHibernate.Linq;
using System;
using System.Linq;

namespace ECO.Providers.NHibernate.Utils;

public static class NHQueryCacheExtensions
{
    public static IQueryable<T> AsCachedQueryable<T, K>(this IReadOnlyRepository<T, K> repository, string regionName) where T : class, IAggregateRoot<K>
    {
        if (repository is null)
        {
            throw new ArgumentNullException(nameof(repository));
        }
        if (regionName is null)
        {
            throw new ArgumentNullException(nameof(regionName));
        }

        return repository.AsQueryable().WithOptions(opt =>
        {
            opt.SetCacheable(true);
            opt.SetCacheRegion(regionName);
        });
    }
}
