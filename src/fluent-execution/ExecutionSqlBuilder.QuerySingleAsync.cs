using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    async Task<dynamic> IExecutionBuilder.QuerySingleAsync(CancellationToken cancellation)
        => await PrepareAndExecuteAsync(this.connection.QuerySingleAsync, cancellation);

    async Task<T> IExecutionBuilder.QuerySingleAsync<T>(CancellationToken cancellation)
        => await PrepareAndExecuteAsync(this.connection.QuerySingleAsync<T>, cancellation);

    async Task<dynamic?> IExecutionBuilder.QuerySingleOrDefaultAsync(CancellationToken cancellation)
        => await PrepareAndExecuteAsync(this.connection.QuerySingleOrDefaultAsync, cancellation);

    public async Task<T?> QuerySingleOrDefaultAsync<T>(CancellationToken cancellation)
        => await PrepareAndExecuteAsync(this.connection.QuerySingleOrDefaultAsync<T?>, cancellation);

}
