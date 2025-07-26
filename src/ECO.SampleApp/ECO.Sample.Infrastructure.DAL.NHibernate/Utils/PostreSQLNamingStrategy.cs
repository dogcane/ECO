using NHibernate.Cfg;

namespace ECO.Sample.Infrastructure.DAL.NHibernate.Utils;

public class PostgreSQLNamingStrategy : INamingStrategy
{
    private const string QUOTED_STRING_VALUE = "\"";

    public string ClassToTableName(string className) => DoubleQuote(className);
    public string PropertyToColumnName(string propertyName) => DoubleQuote(propertyName);
    public string TableName(string tableName) => DoubleQuote(tableName);
    public string ColumnName(string columnName) => DoubleQuote(columnName);
    public string PropertyToTableName(string className,
                                      string propertyName) => DoubleQuote(propertyName);
    public string LogicalColumnName(string columnName,
                                    string propertyName) => string.IsNullOrWhiteSpace(columnName) ?
            DoubleQuote(propertyName) :
            DoubleQuote(columnName);
    private static string DoubleQuote(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return string.Empty;
        if (raw.StartsWith(QUOTED_STRING_VALUE) && raw.EndsWith(QUOTED_STRING_VALUE))
            return raw;
        raw = raw.Replace("`", "");
        return string.Format("\"{0}\"", raw);
    }
}