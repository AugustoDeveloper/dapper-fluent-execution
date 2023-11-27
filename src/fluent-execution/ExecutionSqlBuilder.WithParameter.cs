using System.Data;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    IExecutionBuilder IExecutionBuilder.WithParameter(string parameterName, DbType dbType, object value, int size)
    {
        Parameters.Add(parameterName, value, dbType, size: size);

        return this;
    }

    IExecutionBuilder IExecutionBuilder.WithParameter(bool condition, string parameterName, DbType dbType, object value, int size)
    {
        if (condition)
        {
            Parameters.Add(parameterName, value, dbType, size: size);
        }

        return this;
    }

    IExecutionBuilder IExecutionBuilder.WithParameter(string parameterName, DbType dbType, object value)
    {
        Parameters.Add(parameterName, value, dbType);

        return this;

    }

    IExecutionBuilder IExecutionBuilder.WithParameter(bool condition, string parameterName, DbType dbType, object value)
    {
        if (condition)
        {
            Parameters.Add(parameterName, value, dbType);
        }

        return this;
    }

    IExecutionBuilder IExecutionBuilder.WithParameter(bool condition, string parameterName, object value)
    {
        if (condition)
        {
            Parameters.Add(parameterName, value);
        }
        return this;
    }

    IExecutionBuilder IExecutionBuilder.WithParameter(string parameterName, object value)
    {
        Parameters.Add(parameterName, value);

        return this;
    }

    IExecutionBuilder IExecutionBuilder.WithParameter(bool condition, object values)
    {
        if (condition)
        {
            Parameters.AddDynamicParams(values);
        }

        return this;
    }

    IExecutionBuilder IExecutionBuilder.WithParameter(object values)
    {
        Parameters.AddDynamicParams(values);

        return this;
    }
}
