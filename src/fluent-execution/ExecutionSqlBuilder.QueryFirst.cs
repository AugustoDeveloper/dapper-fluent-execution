using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    dynamic IExecutionBuilder.QueryFirst() => this.connection.QueryFirst<dynamic>(BuildCommandDefinition());

    T IExecutionBuilder.QueryFirst<T>() => this.connection.QueryFirst<T>(BuildCommandDefinition());

    dynamic? IExecutionBuilder.QueryFirstOrDefault() => this.connection.QueryFirst<dynamic?>(BuildCommandDefinition());

    public T? QueryFirstOrDefault<T>() => this.connection.QueryFirstOrDefault<T?>(BuildCommandDefinition());
}
