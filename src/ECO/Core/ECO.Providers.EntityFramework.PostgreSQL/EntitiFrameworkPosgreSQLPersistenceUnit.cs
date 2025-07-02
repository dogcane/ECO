using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ECO.Providers.EntityFramework.PostgreSQL;

public class EntitiFrameworkPosgreSQLPersistenceUnit(string name, ILoggerFactory? loggerFactory = null) : EntityFrameworkPersistenceUnitBase(name, loggerFactory)
{
    #region Consts

    private static readonly string CONNECTIONSTRING_ATTRIBUTE = "connectionString";

    private static readonly string CONNECTIONSTRINGNAME_ATTRIBUTE = "connectionStringName";

    #endregion

    #region Protected_Methods 

    protected override DbContextOptions CreateDbContextOptions(IDictionary<string, string> extendedAttributes, IConfiguration configuration)
    {
        string connectionString = string.Empty;
        if (string.IsNullOrEmpty(connectionString) && extendedAttributes.TryGetValue(CONNECTIONSTRING_ATTRIBUTE, out string? connection))
        {
            connectionString = connection;
        }
        if (string.IsNullOrEmpty(connectionString) && extendedAttributes.TryGetValue(CONNECTIONSTRINGNAME_ATTRIBUTE, out string? connectionName))
        {
            connectionString = configuration.GetConnectionString(connectionName) ?? "";
        }
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ApplicationException(string.Format("The attribute '{0}' or '{1}' were not found in the persistent unit configuration", CONNECTIONSTRING_ATTRIBUTE, CONNECTIONSTRINGNAME_ATTRIBUTE));
        }
        return new DbContextOptionsBuilder()
            .UseNpgsql(connectionString)
            .Options;
    }

    #endregion
}
