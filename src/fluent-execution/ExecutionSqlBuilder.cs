using System.Data;
using System.Text;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

//FIXME: All Asynchronous fuction need to use ConfigureAwait(false)
//TODO: We need add a way to query (multiple) of many differents types as splitted way
//TODO: We need add a function fill a table value as parameter
internal partial class ExecutionSqlBuilder : IExecutionBuilder, IDisposable
{
    private readonly IDbConnection connection;
    private readonly StringBuilder sqlBuilder;
    private readonly bool shouldDisposeConnection;
    private readonly object parametersLock;

    private DynamicParameters? parameters;
    private IDbTransaction? transaction;
    private CommandType commandType;
    private TimeSpan? commandTimeout;
    private bool disposed;

    private DynamicParameters Parameters => GetParameters();

    private ExecutionSqlBuilder(string rootSql, bool shouldDisposeConnection, IDbConnection connection)
    {
        this.connection = connection;
        this.sqlBuilder = new(rootSql);
        this.commandType = CommandType.Text;
        this.shouldDisposeConnection = shouldDisposeConnection;
        this.parametersLock = new();
    }

    internal static IExecutionBuilder New(string? sql, bool shouldDisposeConnection, IDbConnection? connection)
    {
        if (string.IsNullOrWhiteSpace(sql))
        {
            throw new ArgumentException("Invalid SQL script", nameof(sql));
        }

#if NET6_OR_GREATER
        ArgumentNullException.ThrowIfNull(connection);
#else
        _ = connection ?? throw new ArgumentNullException(nameof(connection));
#endif
        return new ExecutionSqlBuilder(sql!, shouldDisposeConnection, connection!);
    }

    private DynamicParameters GetParameters()
    {
        //We don't want enter in lock every time 
        //to get parameters, so we just duplicate
        //the condition, when parameter is null
        //in case the parameter will be changed.
        //That way, we avoiding race condition
        if (parameters is null)
        {
            lock (parametersLock)
            {
                if (parameters is null)
                {
                    parameters = new();
                }
            }
        }

        return parameters;
    }

    private int? GetCommandTimeout()
    {
        if (!commandTimeout.HasValue)
        {
            return null;
        }

        if (commandTimeout.Value.Seconds < 1)
        {
            return null;
        }

        return commandTimeout.Value.Seconds;
    }

    private CommandDefinition BuildCommandDefinition(CancellationToken cancellation = default)
        => new CommandDefinition(sqlBuilder.ToString(), parameters, transaction, GetCommandTimeout(), commandType, cancellationToken: cancellation);

    IExecutionBuilder IExecutionBuilder.WithTransaction(IDbTransaction transaction)
    {
        this.transaction = transaction;

        return this;
    }

    IExecutionBuilder IExecutionBuilder.AsStoredProcedure()
    {
        if (commandType != CommandType.StoredProcedure)
        {
            commandType = CommandType.StoredProcedure;
        }

        return this;
    }

    IExecutionBuilder IExecutionBuilder.WithCommandTimeout(TimeSpan timeout)
    {
        this.commandTimeout = timeout;
        return this;
    }

    private async Task<T> PrepareAndExecuteAsync<T>(Func<CommandDefinition, Task<T>> dapperFunction, CancellationToken cancellation)
    {
        CommandDefinition def = BuildCommandDefinition(cancellation);
        try
        {
            var result = await dapperFunction.Invoke(def);
            return result;
        }
        catch
        {
            throw;
        }
        finally
        {
            Dispose();

        }
    }

    private T PrepareAndExecute<T>(Func<CommandDefinition, T> dapperFunction)
    {
        CommandDefinition def = BuildCommandDefinition();
        try
        {
            var result = dapperFunction.Invoke(def);
            return result;
        }
        catch
        {
            throw;
        }
        finally
        {
            Dispose();

        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                sqlBuilder.Clear();
                parameters = null;

                if (shouldDisposeConnection)
                {
                    connection?.Close();
                    connection?.Dispose();
                }
            }

            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
