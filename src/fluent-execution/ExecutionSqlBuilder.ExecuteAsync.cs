using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    Task<int> IExecutionBuilder.ExecuteAsync(CancellationToken cancellation)
        => PrepareAndExecuteAsync(this.connection.ExecuteAsync, cancellation);
}
