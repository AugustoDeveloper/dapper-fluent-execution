using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    object IExecutionBuilder.ExecuteScalar()
        => this.connection.ExecuteScalar(BuildCommandDefinition());

    T IExecutionBuilder.ExecuteScalar<T>()
        => this.connection.ExecuteScalar<T>(BuildCommandDefinition());
}
