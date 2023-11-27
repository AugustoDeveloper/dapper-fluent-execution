using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    async Task<dynamic> IExecutionBuilder.QuerySingleAsync(CancellationToken cancellation) => await this.connection.QuerySingleAsync(BuildCommandDefinition(cancellation));

    async Task<T> IExecutionBuilder.QuerySingleAsync<T>(CancellationToken cancellation) => await this.connection.QuerySingleAsync<T>(BuildCommandDefinition(cancellation));

    async Task<dynamic?> IExecutionBuilder.QuerySingleOrDefaultAsync(CancellationToken cancellation)
    {
        var result = await this.connection.QuerySingleOrDefaultAsync(BuildCommandDefinition(cancellation));

        if (result is not null)
        {
            return result;
        }

        return default;
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(CancellationToken cancellation)
    {
        var result = await this.connection.QuerySingleOrDefaultAsync<T?>(BuildCommandDefinition(cancellation));

        if (result is not null)
        {
            return result;
        }

        return default;
    }
}
