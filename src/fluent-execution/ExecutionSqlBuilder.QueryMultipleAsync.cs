using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    async Task<T> IExecutionBuilder.QueryMultipleAsync<T>(Func<SqlMapper.GridReader, Task<T>> funcTask, CancellationToken cancellation)
    {
        using var gridReader = await this.connection.QueryMultipleAsync(BuildCommandDefinition(cancellation));
        return await funcTask(gridReader);
    }
}
