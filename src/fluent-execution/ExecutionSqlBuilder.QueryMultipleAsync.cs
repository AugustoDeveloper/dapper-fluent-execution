using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    async Task<T> IExecutionBuilder.QueryMultipleAsync<T>(Func<SqlMapper.GridReader, Task<T>> funcTask, CancellationToken cancellation)
    {
        Func<CommandDefinition, Task<T>> queryMultipleAsyncFunc = async (CommandDefinition def) =>
        {
            using var gridReader = await this.connection.QueryMultipleAsync(def);
            return await funcTask(gridReader);
        };

        return await PrepareAndExecuteAsync(queryMultipleAsyncFunc, cancellation);
    }
}
