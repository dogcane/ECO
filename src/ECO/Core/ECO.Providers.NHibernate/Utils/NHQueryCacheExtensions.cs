namespace ECO.Providers.NHibernate.Utils;

using global::NHibernate.Linq;
using System;
using System.Linq;

public static class NHQueryCacheExtensions
{
    public static IQueryable<T> AsCachedQueryable<T, K>(this IReadOnlyRepository<T, K> repository, string regionName)
        where T : class, IAggregateRoot<K>
        where K : notnull, IEquatable<K>
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(regionName);

        return repository.AsQueryable().WithOptions(opt =>
        {
            opt.SetCacheable(true);
            opt.SetCacheRegion(regionName);
        });
    }
}
