using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    dynamic IExecutionBuilder.QueryFirst()
        => PrepareAndExecute(this.connection.QueryFirst<dynamic>);

    T IExecutionBuilder.QueryFirst<T>()
        => PrepareAndExecute(this.connection.QueryFirst<T>);

    dynamic? IExecutionBuilder.QueryFirstOrDefault()
        => PrepareAndExecute(this.connection.QueryFirst<dynamic?>);

    public T? QueryFirstOrDefault<T>()
        => PrepareAndExecute(this.connection.QueryFirstOrDefault<T?>);
}
