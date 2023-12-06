using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    T IExecutionBuilder.QueryMultiple<T>(Func<SqlMapper.GridReader, T> func)
    {
        Func<CommandDefinition, T> queryMultipleFunc = (CommandDefinition def) =>
        {
            using var gridreader = this.connection.QueryMultiple(def);
            return func(gridreader);
        };

        return PrepareAndExecute(queryMultipleFunc);
    }
}
