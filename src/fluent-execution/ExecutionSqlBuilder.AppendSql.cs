using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    IExecutionBuilder IExecutionBuilder.AppendSql(string sql)
    {
        sqlBuilder.AppendLine(sql);

        return this;
    }

    IExecutionBuilder IExecutionBuilder.AppendSql(bool condition, string sql)
    {
        if (condition)
        {
            sqlBuilder.AppendLine(sql);
        }

        return this;
    }

    IExecutionBuilder IExecutionBuilder.AppendSql(bool condition, string sql, Action<IParameterActionBuilder> builder)
    {
        if (condition)
        {
            sqlBuilder.AppendLine(sql);
            builder.Invoke(ParameterBuilder);
        }

        return this;
    }
}
