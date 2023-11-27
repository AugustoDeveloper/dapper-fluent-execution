using System.Data;
using System.Text;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

//FIXME: All connection execution should dispose this class instance
//FIXME: All Asynchronous fuction need to use ConfigureAwait(false)
//TODO: We need add a way to query (multiple) of many differents types
internal partial class ExecutionSqlBuilder : IExecutionBuilder, IDisposable
{
    private readonly IDbConnection connection;
    private readonly StringBuilder sqlBuilder;
    private readonly bool shouldDisposeConnection;

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
        if (parameters is null)
        {
            parameters = new();
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

    IExecutionBuilder IExecutionBuilder.WithCommandTimeout(TimeSpan timeout)
    {
        this.commandTimeout = timeout;
        return this;
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
