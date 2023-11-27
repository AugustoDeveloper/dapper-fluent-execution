using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    int IExecutionBuilder.Execute()
        => this.connection.Execute(BuildCommandDefinition());
}
