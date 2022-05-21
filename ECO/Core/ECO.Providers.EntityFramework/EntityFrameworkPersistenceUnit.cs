using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO;
using ECO.Data;

namespace ECO.Providers.EntityFramework
{
    public class EntityFrameworkPersistenceUnit : PersistenceUnitBase
    {
        #region Consts

        private static readonly string DBCONTEXTTYPE_ATTRIBUTE = "dbContextType";

        private static readonly string CONNECTIONSTRING_ATTRIBUTE = "connectionString";

        #endregion

        #region Private_Fields

        private Type _DbContextType;

        private string _ConnectionString;

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
            if (extendedAttributes.ContainsKey(CONNECTIONSTRING_ATTRIBUTE))
            {
                _ConnectionString = extendedAttributes[CONNECTIONSTRING_ATTRIBUTE];
            }
            else
            {
                throw new ApplicationException(string.Format("The attribute '{0}' was not found in the persistent unit configuration", CONNECTIONSTRING_ATTRIBUTE));
            }
        }

        protected override IPersistenceContext CreateContext()
        {
            DbContext context = Activator.CreateInstance(_DbContextType, _ConnectionString) as DbContext;
            return new EntityFrameworkPersistenceContext(context);
        }

        public override IReadOnlyRepository<T, K> BuildReadOnlyRepository<T, K>()
        {
            return new EntityFrameworkReadOnlyRepository<T, K>();
        }

        public override IRepository<T, K> BuildRepository<T, K>()
        {
            return new EntityFrameworkRepository<T, K>();
        }

        #endregion
    }
}
