using System.Data;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

public static class StringExtensions
{
    public static IExecutionBuilder On(this string? sql, IDbConnection? connection)
    {
        return ExecutionSqlBuilder.New(sql, connection);
    }
}
