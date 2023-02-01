using System.Data;
using System.Text;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal class ExecutionSqlBuilder : IExecutionBuilder, IDisposable
{
    public bool Disposed { get; private set; }
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
        // https://learn.microsoft.com/en-us/dotnet/standard/frameworks
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(connection);
#else
        _ = connection ?? throw new ArgumentNullException(nameof(connection));
#endif
        return new ExecutionSqlBuilder(sql!, connection!);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!Disposed)
        {
            if (disposing)
            {
                sqlBuilder.Clear();

                Disposed = true;
            }
        }
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

        if (commandTimeout.Value.TotalSeconds < 1)
        {
            return null;
        }

        return (int)commandTimeout.Value.TotalSeconds;
    }

    private CommandDefinition BuildCommandDefinition(CancellationToken cancellation = default)
        => new CommandDefinition(sqlBuilder.ToString(), parameters, transaction, GetCommandTimeout(), commandType, cancellationToken: cancellation);

    protected async Task<T> PerformDapperFunctionAsync<T>(Func<IDbConnection, CommandDefinition, Task<T>> dapperFunc, CancellationToken cancellation)
    {
        var result = await dapperFunc(connection, BuildCommandDefinition(cancellation));
        Dispose();

        return result;
    }

    protected T PerformDapperFunction<T>(Func<IDbConnection, CommandDefinition, T> dapperFunc)
    {
        var result = dapperFunc(connection, BuildCommandDefinition());
        Dispose();

        return result;
    }

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

    IEnumerable<dynamic> IExecutionBuilder.Query() => PerformDapperFunction((c, d) => c.Query<dynamic>(d));

    IEnumerable<T> IExecutionBuilder.Query<T>() => PerformDapperFunction((c, d) => c.Query<T>(d));

    dynamic IExecutionBuilder.QueryFirst() => PerformDapperFunction((c, d) => c.QueryFirst<dynamic>(d));

    T IExecutionBuilder.QueryFirst<T>() => PerformDapperFunction((c, d) => c.QueryFirst<T>(d));

    dynamic IExecutionBuilder.QuerySingle() => PerformDapperFunction((c, d) => c.QuerySingle<dynamic>(d));

    T IExecutionBuilder.QuerySingle<T>() => PerformDapperFunction((c, d) => c.QuerySingle<T>(d));

    dynamic? IExecutionBuilder.QueryFirstOrDefault() => PerformDapperFunction((c, d) => c.QueryFirstOrDefault<dynamic>(d));

    public T? QueryFirstOrDefault<T>() => PerformDapperFunction((c, d) => c.QueryFirstOrDefault<T>(d));

    dynamic? IExecutionBuilder.QuerySingleOrDefault() => PerformDapperFunction((c, d) => c.QuerySingleOrDefault<dynamic>(d));

    public T? QuerySingleOrDefault<T>() => PerformDapperFunction((c, d) => c.QuerySingleOrDefault<T>(d));

    T IExecutionBuilder.QueryMultiple<T>(Func<SqlMapper.GridReader, T> func)
        => PerformDapperFunction((c, d) =>
        {
            using var gridreader = this.connection.QueryMultiple(BuildCommandDefinition());
            return func(gridreader);
        });

    async Task<IEnumerable<T>> IExecutionBuilder.QueryAsync<T>(CancellationToken cancellation) => await PerformDapperFunction((c, d) => c.QueryAsync<T>(d));

    async Task<IEnumerable<dynamic>> IExecutionBuilder.QueryAsync(CancellationToken cancellation) => await PerformDapperFunction((c, d) => c.QueryAsync<dynamic>(d));

    async Task<dynamic> IExecutionBuilder.QuerySingleAsync(CancellationToken cancellation) => await PerformDapperFunctionAsync((c, d) => c.QuerySingleAsync<dynamic>(d), cancellation);

    async Task<T> IExecutionBuilder.QuerySingleAsync<T>(CancellationToken cancellation) => await PerformDapperFunctionAsync((c, d) => c.QuerySingleAsync<T>(d), cancellation);

    async Task<dynamic?> IExecutionBuilder.QuerySingleOrDefaultAsync(CancellationToken cancellation) => await PerformDapperFunctionAsync((c, d) => c.QuerySingleOrDefaultAsync<dynamic>(d), cancellation);

    public async Task<T?> QuerySingleOrDefaultAsync<T>(CancellationToken cancellation) => await PerformDapperFunctionAsync((c, d) => c.QuerySingleOrDefaultAsync<T>(d), cancellation);

    async Task<dynamic> IExecutionBuilder.QueryFirstAsync(CancellationToken cancellation) => await PerformDapperFunctionAsync((c, d) => c.QueryFirstAsync<dynamic>(d), cancellation);

    async Task<T> IExecutionBuilder.QueryFirstAsync<T>(CancellationToken cancellation) => await PerformDapperFunctionAsync((c, d) => c.QueryFirstAsync<T>(d), cancellation);

    async Task<dynamic?> IExecutionBuilder.QueryFirstOrDefaultAsync(CancellationToken cancellation) => await PerformDapperFunctionAsync((c, d) => c.QueryFirstOrDefaultAsync<dynamic>(d), cancellation);

    public async Task<T?> QueryFirstOrDefaultAsync<T>(CancellationToken cancellation) => await PerformDapperFunctionAsync((c, d) => c.QueryFirstOrDefaultAsync<T>(d), cancellation);

    async Task<T> IExecutionBuilder.QueryMultipleAsync<T>(Func<SqlMapper.GridReader, Task<T>> funcTask, CancellationToken cancellation)
        => await PerformDapperFunctionAsync((c, d) =>
        {
            using var gridreader = this.connection.QueryMultiple(BuildCommandDefinition());
            return funcTask(gridreader);
        }, cancellation);

    int IExecutionBuilder.Execute() => PerformDapperFunction((c, d) => c.Execute(d));

    async Task<int> IExecutionBuilder.ExecuteAsync(CancellationToken cancellation) => await PerformDapperFunctionAsync((c, d) => c.ExecuteAsync(BuildCommandDefinition(cancellation)), cancellation);

    object IExecutionBuilder.ExecuteScalar() => PerformDapperFunction((c, d) => c.ExecuteScalar(d));

    T IExecutionBuilder.ExecuteScalar<T>() => PerformDapperFunction((c, d) => c.ExecuteScalar<T>(d));

    async Task<object> IExecutionBuilder.ExecuteScalarAsync(CancellationToken cancellation) => await PerformDapperFunctionAsync((c, d) => c.ExecuteScalarAsync(d), cancellation);

    async Task<T> IExecutionBuilder.ExecuteScalarAsync<T>(CancellationToken cancellation) => await PerformDapperFunctionAsync((c, d) => c.ExecuteScalarAsync<T>(d), cancellation);

    IDataReader IExecutionBuilder.ExecuteDataReader() => PerformDapperFunction((c, d) => c.ExecuteReader(d));

    async Task<IDataReader> IExecutionBuilder.ExecuteDataReaderAsync(CancellationToken cancellation) => await PerformDapperFunctionAsync((c, d) => c.ExecuteReaderAsync(d), cancellation);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
