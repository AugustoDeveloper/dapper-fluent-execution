using System.Data;
using System.Text;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal class ExecutionSqlBuilder : IExecutionBuilder
{
    private readonly IDbConnection connection;
    private readonly StringBuilder sqlBuilder;
    private DynamicParameters? parameters;
    private IDbTransaction? transaction;
    private CommandType commandType;
    private TimeSpan? commandTimeout;

    private DynamicParameters Parameters => GetParameters();

    protected ExecutionSqlBuilder(string rootSql, IDbConnection connection)
    {
        this.connection = connection;
        this.sqlBuilder = new(rootSql);
        this.commandType = CommandType.Text;
    }

    internal static IExecutionBuilder New(string? sql, IDbConnection? connection)
    {
        if (string.IsNullOrWhiteSpace(sql))
        {
            throw new ArgumentException("Invalid SQL script", nameof(sql));
        }

#if NETSTANDARD2_0
        _ = connection ?? throw new ArgumentNullException(nameof(connection));
#elif NETSTANDARD2_1
        _ = connection ?? throw new ArgumentNullException(nameof(connection));
#else
        ArgumentNullException.ThrowIfNull(connection);
#endif
        return new ExecutionSqlBuilder(sql!, connection!);
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

    IExecutionBuilder IExecutionBuilder.AppendSql(bool condition, string sql)
    {
        if (condition)
        {
            sqlBuilder.Append(sql);
        }

        return this;
    }

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

    IExecutionBuilder IExecutionBuilder.WithCommandTimeout(TimeSpan timeout)
    {
        this.commandTimeout = timeout;
        return this;
    }

    IEnumerable<dynamic> IExecutionBuilder.Query() => this.connection.Query<dynamic>(BuildCommandDefinition());

    IEnumerable<T> IExecutionBuilder.Query<T>() => this.connection.Query<T>(BuildCommandDefinition());

    dynamic IExecutionBuilder.QueryFirst() => this.connection.QueryFirst<dynamic>(BuildCommandDefinition());

    T IExecutionBuilder.QueryFirst<T>() => this.connection.QueryFirst<T>(BuildCommandDefinition());

    dynamic IExecutionBuilder.QuerySingle() => this.connection.QuerySingle<dynamic>(BuildCommandDefinition());

    T IExecutionBuilder.QuerySingle<T>() => this.connection.QuerySingle<T>(BuildCommandDefinition());

    dynamic? IExecutionBuilder.QueryFirstOrDefault() => this.connection.QueryFirst<dynamic?>(BuildCommandDefinition());

    public T? QueryFirstOrDefault<T>() => this.connection.QueryFirstOrDefault<T?>(BuildCommandDefinition());

    dynamic? IExecutionBuilder.QuerySingleOrDefault() => this.connection.QueryFirst<dynamic?>(BuildCommandDefinition());

    public T? QuerySingleOrDefault<T>() => this.connection.QueryFirstOrDefault<T?>(BuildCommandDefinition());

    T IExecutionBuilder.QueryMultiple<T>(Func<SqlMapper.GridReader, T> func)
    {
        using var gridreader = this.connection.QueryMultiple(BuildCommandDefinition());
        return func(gridreader);
    }

    AsyncResult<T> IExecutionBuilder.QueryAsync<T>(CancellationToken cancellation)
    {
        var result = this.connection.QueryAsync<T>(BuildCommandDefinition(cancellation));

        return new(result);
    }

    AsyncResult<dynamic> IExecutionBuilder.QueryAsync(CancellationToken cancellation)
    {
        var result = this.connection.QueryAsync(BuildCommandDefinition(cancellation));

        return new(result);
    }

    async Task<dynamic> IExecutionBuilder.QuerySingleAsync(CancellationToken cancellation) => await this.connection.QuerySingleAsync(BuildCommandDefinition(cancellation));

    async Task<T> IExecutionBuilder.QuerySingleAsync<T>(CancellationToken cancellation) => await this.connection.QuerySingleAsync<T>(BuildCommandDefinition(cancellation));

    async Task<dynamic?> IExecutionBuilder.QuerySingleOrDefaultAsync(CancellationToken cancellation)
    {
        var result = await this.connection.QuerySingleOrDefaultAsync(BuildCommandDefinition(cancellation));

        if (result is not null)
        {
            return result;
        }

        return default;
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(CancellationToken cancellation)
    {
        var result = await this.connection.QuerySingleOrDefaultAsync<T?>(BuildCommandDefinition(cancellation));

        if (result is not null)
        {
            return result;
        }

        return default;

    }

    async Task<dynamic> IExecutionBuilder.QueryFirstAsync(CancellationToken cancellation) => await this.connection.QueryFirstAsync(BuildCommandDefinition(cancellation));

    async Task<T> IExecutionBuilder.QueryFirstAsync<T>(CancellationToken cancellation) => await this.connection.QueryFirstAsync<T>(BuildCommandDefinition(cancellation));

    async Task<dynamic?> IExecutionBuilder.QueryFirstOrDefaultAsync(CancellationToken cancellation)
    {
        var result = await this.connection.QueryFirstOrDefaultAsync(BuildCommandDefinition(cancellation));

        if (result is not null)
        {
            return result;
        }

        return default;
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(CancellationToken cancellation)
    {
        var result = await this.connection.QueryFirstOrDefaultAsync<T?>(BuildCommandDefinition(cancellation));

        if (result is not null)
        {
            return result;
        }

        return default;
    }

    async Task<T> IExecutionBuilder.QueryMultipleAsync<T>(Func<SqlMapper.GridReader, Task<T>> funcTask, CancellationToken cancellation)
    {
        using var gridReader = await this.connection.QueryMultipleAsync(BuildCommandDefinition(cancellation));
        return await funcTask(gridReader);
    }

    int IExecutionBuilder.Execute() => this.connection.Execute(BuildCommandDefinition());

    async Task<int> IExecutionBuilder.ExecuteAsync(CancellationToken cancellation) => await this.connection.ExecuteAsync(BuildCommandDefinition(cancellation));

    object IExecutionBuilder.ExecuteScalar() => this.connection.ExecuteScalar(BuildCommandDefinition());

    T IExecutionBuilder.ExecuteScalar<T>() => this.connection.ExecuteScalar<T>(BuildCommandDefinition());

    async Task<object> IExecutionBuilder.ExecuteScalarAsync(CancellationToken cancellation) => await this.connection.ExecuteScalarAsync(BuildCommandDefinition(cancellation));

    async Task<T> IExecutionBuilder.ExecuteScalarAsync<T>(CancellationToken cancellation) => await this.connection.ExecuteScalarAsync<T>(BuildCommandDefinition(cancellation));

    IDataReader IExecutionBuilder.ExecuteDataReader() => this.connection.ExecuteReader(BuildCommandDefinition());

    async Task<IDataReader> IExecutionBuilder.ExecuteDataReaderAsync(CancellationToken cancellation) => await this.connection.ExecuteReaderAsync(BuildCommandDefinition(cancellation));
}
