using System.Data;
using Dapper.FluentExecution.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;

namespace Dapper.FluentExecution.SqlServer;

public static class SqlServerExecutionSqlBuilderExtension
{
    /// https://github.com/dotnet/SqlClient/blob/0156df230c4426be535ad6af32ac7d16188377d1/src/Microsoft.Data.SqlClient/src/Microsoft/Data/SqlClient/SqlEnums.cs#L239
    public static DbType ToDbType(this SqlDbType dbType)
        => dbType switch
        {
            SqlDbType.VarChar => DbType.AnsiString,
            SqlDbType.Char => DbType.AnsiStringFixedLength,
            SqlDbType.VarBinary => DbType.Binary,
            SqlDbType.TinyInt => DbType.Byte,
            SqlDbType.Bit => DbType.Boolean,
            SqlDbType.Money => DbType.Currency,
            SqlDbType.Date => DbType.Date,
            SqlDbType.DateTime => DbType.DateTime,
            SqlDbType.Decimal => DbType.Decimal,
            SqlDbType.Float => DbType.Double,
            SqlDbType.UniqueIdentifier => DbType.Guid,
            SqlDbType.SmallInt => DbType.Int16,
            SqlDbType.Int => DbType.Int32,
            SqlDbType.BigInt => DbType.Int64,
            SqlDbType.Variant => DbType.Object,
            SqlDbType.Real => DbType.Single,
            SqlDbType.NVarChar => DbType.String,
            SqlDbType.NChar => DbType.StringFixedLength,
            SqlDbType.Time => DbType.Time,
            SqlDbType.Xml => DbType.Xml,
            SqlDbType.DateTime2 => DbType.DateTime2,
            SqlDbType.DateTimeOffset => DbType.DateTimeOffset,

            _ => throw new NotSupportedException($"{dbType} is not supported")
        };

    public static IExecutionBuilder WithSqlParameter(this IExecutionBuilder builder, bool condition, string parameterName, SqlDbType dbType, object value)
        => builder.WithParameter(condition, $"@{parameterName}", dbType.ToDbType(), value);

    public static IExecutionBuilder WithSqlParameter(this IExecutionBuilder builder, bool condition, string parameterName, SqlDbType dbType, object value, int size)
        => builder.WithParameter(condition, $"@{parameterName}", dbType.ToDbType(), value, size);

    public static IExecutionBuilder WithSqlParameter(this IExecutionBuilder builder, string parameterName, SqlDbType dbType, object value)
        => builder.WithParameter($"@{parameterName}", dbType.ToDbType(), value);

    public static IExecutionBuilder WithSqlParameter(this IExecutionBuilder builder, string parameterName, SqlDbType dbType, object value, int size)
        => builder.WithParameter($"@{parameterName}", dbType.ToDbType(), value, size);
}
