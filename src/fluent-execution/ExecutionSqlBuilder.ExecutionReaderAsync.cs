using System.Data;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    async Task<IDataReader> IExecutionBuilder.ExecuteDataReaderAsync(CancellationToken cancellation)
        => await this.connection.ExecuteReaderAsync(BuildCommandDefinition(cancellation));
}
