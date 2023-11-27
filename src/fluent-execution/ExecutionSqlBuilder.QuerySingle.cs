using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    dynamic IExecutionBuilder.QuerySingle() => this.connection.QuerySingle<dynamic>(BuildCommandDefinition());

    T IExecutionBuilder.QuerySingle<T>() => this.connection.QuerySingle<T>(BuildCommandDefinition());

    dynamic? IExecutionBuilder.QuerySingleOrDefault() => this.connection.QueryFirst<dynamic?>(BuildCommandDefinition());

    public T? QuerySingleOrDefault<T>() => this.connection.QueryFirstOrDefault<T?>(BuildCommandDefinition());
}
