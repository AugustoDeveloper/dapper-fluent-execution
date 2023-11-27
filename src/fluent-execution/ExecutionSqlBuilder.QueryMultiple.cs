using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    T IExecutionBuilder.QueryMultiple<T>(Func<SqlMapper.GridReader, T> func)
    {
        using var gridreader = this.connection.QueryMultiple(BuildCommandDefinition());
        return func(gridreader);
    }
}
