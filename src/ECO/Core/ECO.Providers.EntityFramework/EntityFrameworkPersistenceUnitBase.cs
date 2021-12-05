using ECO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECO.Providers.EntityFramework
{
    public abstract class EntityFrameworkPersistenceUnitBase : PersistenceUnitBase<EntityFrameworkPersistenceUnitBase>
    {
        #region Consts

        protected static readonly string DBCONTEXTTYPE_ATTRIBUTE = "dbContextType";

        #endregion

        #region Private_Fields

        protected Type _DbContextType;

        protected DbContextOptions _DbContextOptions;

        #endregion

        #region Ctor

        protected EntityFrameworkPersistenceUnitBase(string name, ILoggerFactory loggerFactory)
            : base(name, loggerFactory)
        {

        }

        #endregion

        #region Protected_Methods

        protected abstract DbContextOptions CreateDbContextOptions(IDictionary<string, string> extendedAttributes);

        #endregion

        #region PersistenceUnitBase

        protected override void OnInitialize(IDictionary<string, string> extendedAttributes)
        {
            base.OnInitialize(extendedAttributes);
            if (extendedAttributes.ContainsKey(DBCONTEXTTYPE_ATTRIBUTE))
            {
                _DbContextType = Type.GetType(extendedAttributes[DBCONTEXTTYPE_ATTRIBUTE]);
            }
            else
            {
                throw new ApplicationException(string.Format("The attribute '{0}' was not found in the persistent unit configuration", DBCONTEXTTYPE_ATTRIBUTE));
            }
            _DbContextOptions = CreateDbContextOptions(extendedAttributes);
            //Register class types
            using (DbContext context = Activator.CreateInstance(_DbContextType, _DbContextOptions) as DbContext)
            {
                foreach(var entity in context.Model.GetEntityTypes())
                {
                    var entityType = entity.ClrType;
                    if (entityType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAggregateRoot<>)))
                        _Classes.Add(entity.ClrType);
                }
            }

        }


        protected override IPersistenceContext OnCreateContext()
        {
            DbContext context = Activator.CreateInstance(_DbContextType, _DbContextOptions) as DbContext;            
            return new EntityFrameworkPersistenceContext(this, _LoggerFactory.CreateLogger<EntityFrameworkPersistenceContext>(), context);
        }

        public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>(IDataContext dataContext)
        {
            return new EntityFrameworkReadOnlyRepository<T, K>(dataContext);
        }

        public override IRepository<T, K> BuildRepository<T, K>(IDataContext dataContext)
        {
            return new EntityFrameworkRepository<T, K>(dataContext);
        }

        #endregion
    }
}
