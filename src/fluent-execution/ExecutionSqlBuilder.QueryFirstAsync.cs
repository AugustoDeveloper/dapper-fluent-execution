using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    async Task<dynamic> IExecutionBuilder.QueryFirstAsync(CancellationToken cancellation)
        => await PrepareAndExecuteAsync(this.connection.QueryFirstAsync, cancellation).ConfigureAwait(false);

    async Task<T> IExecutionBuilder.QueryFirstAsync<T>(CancellationToken cancellation)
        => await PrepareAndExecuteAsync(this.connection.QueryFirstAsync<T>, cancellation).ConfigureAwait(false);

    async Task<dynamic?> IExecutionBuilder.QueryFirstOrDefaultAsync(CancellationToken cancellation)
        => await PrepareAndExecuteAsync(this.connection.QueryFirstOrDefaultAsync, cancellation).ConfigureAwait(false);

    public async Task<T?> QueryFirstOrDefaultAsync<T>(CancellationToken cancellation)
        => await PrepareAndExecuteAsync(this.connection.QueryFirstOrDefaultAsync<T?>, cancellation).ConfigureAwait(false);
}
