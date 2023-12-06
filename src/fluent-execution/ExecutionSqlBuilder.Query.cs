using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    IEnumerable<dynamic> IExecutionBuilder.Query()
        => PrepareAndExecute(this.connection.Query<dynamic>);

    IEnumerable<T> IExecutionBuilder.Query<T>()
        => PrepareAndExecute(this.connection.Query<T>);
}
