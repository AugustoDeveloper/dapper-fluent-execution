using System.Data;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal class DynamicParameterBuilder : IParameterableBuilder
{
    private DynamicParameters? parameters;
    private bool disposed;

    private DynamicParameters Parameters => disposed
        ? throw new ObjectDisposedException(nameof(parameters))
        : parameters!;

    internal DynamicParameterBuilder()
    {
        parameters = new();
    }

    DynamicParameters IParameterableBuilder.Build()
    {
        return Parameters;
    }

    IParameterActionBuilder IParameterActionBuilder.WithParameter(string parameterName, DbType dbType, object value, int size)
    {
        Parameters.Add(parameterName, value, dbType, size: size);

        return this;
    }

    IParameterActionBuilder IParameterActionBuilder.WithParameter(bool condition, string parameterName, DbType dbType, object value, int size)
    {
        if (condition)
        {
            Parameters.Add(parameterName, value, dbType, size: size);
        }

        return this;
    }

    IParameterActionBuilder IParameterActionBuilder.WithParameter(string parameterName, DbType dbType, object value)
    {
        Parameters.Add(parameterName, value, dbType);

        return this;

    }

    IParameterActionBuilder IParameterActionBuilder.WithParameter(bool condition, string parameterName, DbType dbType, object value)
    {
        if (condition)
        {
            Parameters.Add(parameterName, value, dbType);
        }

        return this;
    }

    IParameterActionBuilder IParameterActionBuilder.WithParameter(bool condition, string parameterName, object value)
    {
        if (condition)
        {
            Parameters.Add(parameterName, value);
        }
        return this;
    }

    IParameterActionBuilder IParameterActionBuilder.WithParameter(string parameterName, object value)
    {
        Parameters.Add(parameterName, value);

        return this;
    }

    IParameterActionBuilder IParameterActionBuilder.WithParameter(bool condition, object values)
    {
        if (condition)
        {
            Parameters.AddDynamicParams(values);
        }

        return this;
    }

    IParameterActionBuilder IParameterActionBuilder.WithParameter(object values)
    {
        Parameters.AddDynamicParams(values);

        return this;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                parameters = null;
            }

            disposed = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
