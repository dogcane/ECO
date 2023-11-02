using ECO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.Providers.EntityFramework
{
    public sealed class EntityFrameworkPersistenceUnit<TDbContext> : PersistenceUnitBase<EntityFrameworkPersistenceUnitBase>
        where TDbContext : DbContext
    {
        #region Private_Fields

        private readonly DbContextOptions _DbContextOptions;

        #endregion

        #region Ctor

        public EntityFrameworkPersistenceUnit(string name, DbContextOptions dbContextOptions, ILoggerFactory? loggerFactory = null)
            : base(name, loggerFactory) => _DbContextOptions = dbContextOptions ?? throw new ArgumentNullException(nameof(dbContextOptions));

        #endregion

        #region PersistenceUnitBase

        protected override IPersistenceContext OnCreateContext()
        {
            DbContext context = Activator.CreateInstance(typeof(TDbContext), _DbContextOptions) as DbContext ?? throw new InvalidCastException(nameof(context));
            return new EntityFrameworkPersistenceContext(context, this, _LoggerFactory?.CreateLogger<EntityFrameworkPersistenceContext>());
        }

        public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext) => new EntityFrameworkReadOnlyRepository<T, K>(dataContext);

        public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext) => new EntityFrameworkRepository<T, K>(dataContext);

        #endregion
    }
}
