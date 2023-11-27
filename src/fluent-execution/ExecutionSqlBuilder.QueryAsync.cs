using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    AsyncResult<T> IExecutionBuilder.QueryAsync<T>(CancellationToken cancellation)
    {
        var result = this.connection.QueryAsync<T>(BuildCommandDefinition(cancellation));

        return new(result);
    }

    AsyncResult<dynamic> IExecutionBuilder.QueryAsync(CancellationToken cancellation)
    {
        var result = this.connection.QueryAsync(BuildCommandDefinition(cancellation));

        return new(result);
    }
}
