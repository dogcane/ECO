using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECO.Linq
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paged<T>(this IQueryable<T> source, int page, int pageSize)
        {
            return source
              .Skip((page - 1) * pageSize)
              .Take(pageSize);
        }

        public static T GetByIdentity<T, K>(this IQueryable<T> source, K identity)
            where T : IEntity<K>
        {
            return source.AsQueryable().GetByIdentity(identity);
        }

        public static T GetByIdentity<T, K>(this IEnumerable<T> source, K identity)
            where T : IEntity<K>
        {
            return source
                .Where(entity => entity.Identity.Equals(identity))
                .FirstOrDefault();
        }
    }
}
