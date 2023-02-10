using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ECO.Providers.EntityFramework.PostgreSQL
{
    public class EntitiFrameworkPosgreSQLPersistenceUnit : EntityFrameworkPersistenceUnitBase
    {
        #region Consts

        private static readonly string CONNECTIONSTRING_ATTRIBUTE = "connectionString";

        #endregion

        #region Ctor

        public EntitiFrameworkPosgreSQLPersistenceUnit(string name, ILoggerFactory loggerFactory = null) : base(name, loggerFactory)
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
                .UseNpgsql(connectionString)
                .Options;
        }

        #endregion
    }
}
