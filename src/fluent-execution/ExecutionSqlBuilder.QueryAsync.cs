using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    AsyncResult<T> IExecutionBuilder.QueryAsync<T>(CancellationToken cancellation)
    {
        var result = PrepareAndExecuteAsync(this.connection.QueryAsync<T>, cancellation);

        return new(result);
    }

    AsyncResult<dynamic> IExecutionBuilder.QueryAsync(CancellationToken cancellation)
    {
        var result = PrepareAndExecuteAsync(this.connection.QueryAsync, cancellation);

        return new(result);
    }
}
