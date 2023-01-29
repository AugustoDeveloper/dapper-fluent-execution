using System.Data;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

///<summaru>
/// Extension class to able starts a fluent query execution 
/// on connection
///</summary>
public static class StringExtensions
{
    ///<summaru>
    /// Starts a fluent query execution from <paramref name="sql"/> sql string 
    /// by <paramref name="connection"/> that instantiate a <seealso cref="IExecutionBuilder"/>
    ///</summary>
    ///<param name="sql">SQL text to perform a query on connection</param>
    ///<param name="connection">Database connection that allows to perform query</param>
    ///<returns>An <see cref="IExecutionBuilder"/> instance</returns>
    ///<exception cref="ArgumentException">It could be throws if <paramref name="sql"/> is invalid as null, empty or whitespace value</exception>
    ///<exception cref="ArgumentNullException">It could be throws if <paramref name="connection"/> is invalid as null value<exception>
    public static IExecutionBuilder On(this string? sql, IDbConnection? connection)
    {
        return ExecutionSqlBuilder.New(sql, connection);
    }
}
