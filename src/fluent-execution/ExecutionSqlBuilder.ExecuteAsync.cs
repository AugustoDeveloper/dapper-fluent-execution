using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    async Task<int> IExecutionBuilder.ExecuteAsync(CancellationToken cancellation)
        => await PrepareAndExecuteAsync(this.connection.ExecuteAsync, cancellation).ConfigureAwait(false);
}
