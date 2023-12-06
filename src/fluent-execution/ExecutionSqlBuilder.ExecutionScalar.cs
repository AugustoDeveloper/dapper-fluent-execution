using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    object IExecutionBuilder.ExecuteScalar()
        => PrepareAndExecute(this.connection.ExecuteScalar);

    T IExecutionBuilder.ExecuteScalar<T>()
        => PrepareAndExecute(this.connection.ExecuteScalar<T>);
}
