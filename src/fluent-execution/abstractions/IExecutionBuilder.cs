using System.Data;

namespace Dapper.FluentExecution.Abstractions;

///<summary>
///</summary>
public interface IExecutionBuilder
{
    IExecutionBuilder AppendSql(bool condition, string sql);
    IExecutionBuilder WithTransaction(IDbTransaction transaction);

    IExecutionBuilder WithParameter(string parameterName, DbType dbType, object value, int size);
    IExecutionBuilder WithParameter(bool condition, string parameterName, DbType dbType, object value, int size);
    IExecutionBuilder WithParameter(string parameterName, DbType dbType, object value);
    IExecutionBuilder WithParameter(bool condition, string parameterName, DbType dbType, object value);
    IExecutionBuilder WithParameter(string parameterName, object value);
    IExecutionBuilder WithParameter(bool condition, string parameterName, object value);

    IEnumerable<dynamic> Query();
    IEnumerable<T> Query<T>();

    AsyncResult<dynamic> QueryAsync(CancellationToken cancellation = default);
    AsyncResult<T> QueryAsync<T>(CancellationToken cancellation = default);

    dynamic QuerySingle();
    T QuerySingle<T>();

    Task<dynamic> QuerySingleAsync(CancellationToken cancellation = default);
    Task<T> QuerySingleAsync<T>(CancellationToken cancellation = default);

    dynamic? QuerySingleOrDefault();
    T? QuerySingleOrDefault<T>();

    Task<dynamic?> QuerySingleOrDefaultAsync(CancellationToken cancellation = default);
    Task<T?> QuerySingleOrDefaultAsync<T>(CancellationToken cancellation = default);

    dynamic QueryFirst();
    T QueryFirst<T>();

    Task<dynamic> QueryFirstAsync(CancellationToken cancellation = default);
    Task<T> QueryFirstAsync<T>(CancellationToken cancellation = default);

    dynamic? QueryFirstOrDefault();
    T? QueryFirstOrDefault<T>();

    Task<dynamic?> QueryFirstOrDefaultAsync(CancellationToken cancellation = default);
    Task<T?> QueryFirstOrDefaultAsync<T>(CancellationToken cancellation = default);

    T QueryMultiple<T>(Func<SqlMapper.GridReader, T> func);
    Task<T> QueryMultipleAsync<T>(Func<SqlMapper.GridReader, Task<T>> funcTask, CancellationToken cancellation = default);
}
