using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    dynamic IExecutionBuilder.QuerySingle()
        => PrepareAndExecute(this.connection.QuerySingle<dynamic>);

    T IExecutionBuilder.QuerySingle<T>()
        => PrepareAndExecute(this.connection.QuerySingle<T>);

    dynamic? IExecutionBuilder.QuerySingleOrDefault()
        => PrepareAndExecute(this.connection.QueryFirst<dynamic?>);

    public T? QuerySingleOrDefault<T>()
        => PrepareAndExecute(this.connection.QueryFirstOrDefault<T?>);
}
