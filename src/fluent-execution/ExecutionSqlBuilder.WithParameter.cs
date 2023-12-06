using System.Data;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    IExecutionBuilder IExecutionBuilder.WithParameter(string parameterName, DbType dbType, object value, int size)
    {
        ParameterBuilder.WithParameter(parameterName, dbType, value, size: size);

        return this;
    }

    IExecutionBuilder IExecutionBuilder.WithParameter(bool condition, string parameterName, DbType dbType, object value, int size)
    {
        ParameterBuilder.WithParameter(condition, parameterName, dbType, value, size: size);

        return this;
    }

    IExecutionBuilder IExecutionBuilder.WithParameter(string parameterName, DbType dbType, object value)
    {
        ParameterBuilder.WithParameter(parameterName, dbType, value);

        return this;

    }

    IExecutionBuilder IExecutionBuilder.WithParameter(bool condition, string parameterName, DbType dbType, object value)
    {
        ParameterBuilder.WithParameter(condition, parameterName, dbType, value);

        return this;
    }

    IExecutionBuilder IExecutionBuilder.WithParameter(bool condition, string parameterName, object value)
    {
        ParameterBuilder.WithParameter(condition, parameterName, value);
        return this;
    }

    IExecutionBuilder IExecutionBuilder.WithParameter(string parameterName, object value)
    {
        ParameterBuilder.WithParameter(parameterName, value);

        return this;
    }

    IExecutionBuilder IExecutionBuilder.WithParameter(bool condition, object values)
    {
        ParameterBuilder.WithParameter(condition, values);

        return this;
    }

    IExecutionBuilder IExecutionBuilder.WithParameter(object values)
    {
        ParameterBuilder.WithParameter(values);

        return this;
    }
}
