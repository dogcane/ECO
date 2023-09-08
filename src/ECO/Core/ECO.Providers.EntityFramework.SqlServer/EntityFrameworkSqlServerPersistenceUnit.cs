using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        private static readonly string CONNECTIONSTRINGNAME_ATTRIBUTE = "connectionStringName";

        #endregion

        #region Ctor

        public EntityFrameworkSqlServerPersistenceUnit(string name, ILoggerFactory? loggerFactory = null) : base(name, loggerFactory)
        {

        }

        #endregion

        #region Protected_Methods 

        protected override DbContextOptions CreateDbContextOptions(IDictionary<string, string> extendedAttributes, IConfiguration configuration)
        {
            string connectionString = string.Empty;
            if (string.IsNullOrEmpty(connectionString) && extendedAttributes.ContainsKey(CONNECTIONSTRING_ATTRIBUTE))
            {
                connectionString = extendedAttributes[CONNECTIONSTRING_ATTRIBUTE];
            }
            if (string.IsNullOrEmpty(connectionString) && extendedAttributes.ContainsKey(CONNECTIONSTRINGNAME_ATTRIBUTE))
            {
                connectionString = configuration.GetConnectionString(extendedAttributes[CONNECTIONSTRINGNAME_ATTRIBUTE]) ?? "";
            }
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ApplicationException(string.Format("The attribute '{0}' or '{1}' were not found in the persistent unit configuration", CONNECTIONSTRING_ATTRIBUTE, CONNECTIONSTRINGNAME_ATTRIBUTE));
            }
            return new DbContextOptionsBuilder()
                .UseSqlServer(connectionString)
                .Options;
        }

        #endregion
    }
}
