using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    async Task<dynamic> IExecutionBuilder.QueryFirstAsync(CancellationToken cancellation) => await this.connection.QueryFirstAsync(BuildCommandDefinition(cancellation));

    async Task<T> IExecutionBuilder.QueryFirstAsync<T>(CancellationToken cancellation) => await this.connection.QueryFirstAsync<T>(BuildCommandDefinition(cancellation));

    async Task<dynamic?> IExecutionBuilder.QueryFirstOrDefaultAsync(CancellationToken cancellation)
    {
        var result = await this.connection.QueryFirstOrDefaultAsync(BuildCommandDefinition(cancellation));

        if (result is not null)
        {
            return result;
        }

        return default;
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(CancellationToken cancellation)
    {
        var result = await this.connection.QueryFirstOrDefaultAsync<T?>(BuildCommandDefinition(cancellation));

        if (result is not null)
        {
            return result;
        }

        return default;
    }
}
