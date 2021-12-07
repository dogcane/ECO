using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECO.Providers.EntityFramework.SqlServer
{
    public sealed class EntityFrameworkSqlServerPersistenceUnit : EntityFrameworkPersistenceUnitBase
    {
        #region Consts

        private static readonly string CONNECTIONSTRING_ATTRIBUTE = "connectionString";

        #endregion

        #region Ctor

        public EntityFrameworkSqlServerPersistenceUnit(string name, ILoggerFactory loggerFactory) : base(name, loggerFactory)
        {

        }

        #endregion

        #region Protected_Methods 

        protected override DbContextOptions CreateDbContextOptions(IDictionary<string, string> extendedAttributes)
        {
            string connectionString;
            if (extendedAttributes.ContainsKey(CONNECTIONSTRING_ATTRIBUTE))
            {
                connectionString = extendedAttributes[CONNECTIONSTRING_ATTRIBUTE];
            }
            else
            {
                throw new ApplicationException(string.Format("The attribute '{0}' was not found in the persistent unit configuration", CONNECTIONSTRING_ATTRIBUTE));
            }
            return new DbContextOptionsBuilder()
                .UseSqlServer(connectionString)
                .Options;
        }

        #endregion
    }
}
