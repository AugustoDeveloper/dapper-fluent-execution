using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    async Task<object> IExecutionBuilder.ExecuteScalarAsync(CancellationToken cancellation)
        => await PrepareAndExecuteAsync(this.connection.ExecuteScalarAsync, cancellation);

    async Task<T> IExecutionBuilder.ExecuteScalarAsync<T>(CancellationToken cancellation)
        => await PrepareAndExecuteAsync(this.connection.ExecuteScalarAsync<T>, cancellation);
}
