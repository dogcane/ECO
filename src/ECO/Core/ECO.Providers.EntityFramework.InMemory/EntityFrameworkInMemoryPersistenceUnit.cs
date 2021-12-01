using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ECO.Providers.EntityFramework.InMemory
{
    public sealed class EntityFrameworkInMemoryPersistenceUnit : EntityFrameworkPersistenceUnitBase
    {
        #region Consts

        private static readonly string DATABASENAME_ATTRIBUTE = "databaseName";

        #endregion

        #region Ctor

        public EntityFrameworkInMemoryPersistenceUnit(string name, ILoggerFactory loggerFactory) : base(name, loggerFactory)
        {

        }

        #endregion

        #region Protected_Methods

        protected override DbContextOptions CreateDbContextOptions(IDictionary<string, string> extendedAttributes)
        {
            string databaseName;
            if (extendedAttributes.ContainsKey(DATABASENAME_ATTRIBUTE))
            {
                databaseName = extendedAttributes[DATABASENAME_ATTRIBUTE];
            }
            else
            {
                throw new ApplicationException(string.Format("The attribute '{0}' was not found in the persistent unit configuration", DATABASENAME_ATTRIBUTE));
            }
            return new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName)
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }

        #endregion
    }
}
