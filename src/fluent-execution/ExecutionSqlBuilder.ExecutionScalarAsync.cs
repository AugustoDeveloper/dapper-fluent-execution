using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    async Task<object> IExecutionBuilder.ExecuteScalarAsync(CancellationToken cancellation)
        => await this.connection.ExecuteScalarAsync(BuildCommandDefinition(cancellation));

    async Task<T> IExecutionBuilder.ExecuteScalarAsync<T>(CancellationToken cancellation)
        => await this.connection.ExecuteScalarAsync<T>(BuildCommandDefinition(cancellation));
}
