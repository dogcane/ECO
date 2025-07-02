using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ECO.Providers.EntityFramework.InMemory;

public class EntityFrameworkInMemoryPersistenceUnit(string name, ILoggerFactory? loggerFactory = null) : EntityFrameworkPersistenceUnitBase(name, loggerFactory)
{
    #region Consts

    protected static readonly string DATABASENAME_ATTRIBUTE = "databaseName";

    #endregion

    #region Protected_Methods

    protected override DbContextOptions CreateDbContextOptions(IDictionary<string, string> extendedAttributes, IConfiguration configuration)
    {
        string databaseName;
        if (extendedAttributes.TryGetValue(DATABASENAME_ATTRIBUTE, out string? value))
        {
            databaseName = value;
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
