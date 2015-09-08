using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Providers.EntityFramework.Utils
{
    public static class EntityTypeConfiguratorExtensions
    {
        public static ManyNavigationPropertyConfiguration<T, K> HasManyNonPublic<T, K>(this EntityTypeConfiguration<T> config, string nonPublicCollectionProperty)
            where T : class
            where K : class
        {
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = Expression.PropertyOrField(arg, nonPublicCollectionProperty);
            Expression<Func<T, ICollection<K>>> lambda = Expression.Lambda<Func<T, ICollection<K>>>(expr, arg);            
            return config.HasMany<K>(lambda);
        }
    }
}
