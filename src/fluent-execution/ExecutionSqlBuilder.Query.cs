using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    IEnumerable<dynamic> IExecutionBuilder.Query() => this.connection.Query<dynamic>(BuildCommandDefinition());

    IEnumerable<T> IExecutionBuilder.Query<T>() => this.connection.Query<T>(BuildCommandDefinition());
}
