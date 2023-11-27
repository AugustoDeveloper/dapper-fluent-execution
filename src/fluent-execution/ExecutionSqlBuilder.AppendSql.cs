using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    //TODO: We need a override of this function to allows add some specifics
    //      parameters that ensure the condition
    //TODO: We need add override to allows append sql without condition
    IExecutionBuilder IExecutionBuilder.AppendSql(bool condition, string sql)
    {
        if (condition)
        {
            sqlBuilder.Append(sql);
        }

        return this;
    }

}
