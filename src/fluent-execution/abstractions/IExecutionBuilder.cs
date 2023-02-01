using System.Data;

namespace Dapper.FluentExecution.Abstractions;

///<summary>
///Defines a protocol to makes the execution on
///database connection as fluent with Dapper extensions
///</summary>
public interface IExecutionBuilder
{
    bool Disposed { get; }

    ///<summary>
    ///Appends a SQL script string based on condition argument.
    ///</summary>
    ///<param name="sql">SQL script text</param>
    ///<param name="condition">Indicates wether the SQL script should be appended to execution</param>
    ///<returns>An instance of builder</returns>
    IExecutionBuilder AppendSql(bool condition, string sql);

    ///<summary>
    ///Adds a transaction allows the execution can be commited or not
    ///</summary>
    ///<param name="transaction">Transaction instance to controls the execution on database</param>
    ///<returns>An instance of builder</returns>
    IExecutionBuilder WithTransaction(IDbTransaction transaction);

    ///<summary>
    ///Adds to the fluent execution a command timeout, the value will be converted to
    ///seconds
    ///</summary>
    ///<param name="timeout">Indicates the timeout as <see cref="TimeSpan"/></param>
    ///<returns>An instance of builder</returns>
    IExecutionBuilder WithCommandTimeout(TimeSpan timeout);

    ///<summary>
    ///Indicates the SQL script text is a Stored Procedure execution, until call
    ///this method the SQL script executed as <seealso cref="CommandType.Text"/> type
    ///</summary>
    ///<returns>An instance of builder</returns>
    IExecutionBuilder AsStoredProcedure();

    ///<summary>
    ///Adds a parameter of specific type and size indicating
    ///</summary>
    ///<param name="parameterName">Identificator of parameter on execution</param>
    ///<param name="dbType">Database type of parameter</param>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<param name="size">Size of value on database</param>
    ///<returns>An instance of builder</returns>
    IExecutionBuilder WithParameter(string parameterName, DbType dbType, object value, int size);

    ///<summary>
    ///Adds a parameter of specific type and size indicating, in cases the condition is true.
    ///</summary>
    ///<param name="condition">Indicates wether parameter should be added to execution</param>
    ///<param name="parameterName">Identificator of parameter on execution</param>
    ///<param name="dbType">Database type of parameter</param>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<param name="size">Size of value on database</param>
    ///<returns>An instance of builder</returns>
    IExecutionBuilder WithParameter(bool condition, string parameterName, DbType dbType, object value, int size);

    ///<summary>
    ///Adds a parameter of specific type
    ///</summary>
    ///<param name="parameterName">Identificator of parameter on execution</param>
    ///<param name="dbType">Database type of parameter</param>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<returns>An instance of builder</returns>
    IExecutionBuilder WithParameter(string parameterName, DbType dbType, object value);

    ///<summary>
    ///Adds a parameter of specific type in cases the condition is true.
    ///</summary>
    ///<param name="condition">Indicates wether parameter should be added to execution</param>
    ///<param name="parameterName">Identificator of parameter on execution</param>
    ///<param name="dbType">Database type of parameter</param>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<returns>An instance of builder</returns>
    IExecutionBuilder WithParameter(bool condition, string parameterName, DbType dbType, object value);

    ///<summary>
    ///Adds a parameter with <paramref name="parameterName"/> identificator and
    ///with stored value on <paramref name="value"/>.
    ///</summary>
    ///<param name="parameterName">Identificator of parameter on execution</param>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<returns>An instance of builder</returns>
    IExecutionBuilder WithParameter(string parameterName, object value);

    ///<summary>
    ///Adds a parameter with <paramref name="parameterName"/> identificator and
    ///with stored value on <paramref name="value"/>, in cases the <paramref name="condition"/> 
    ///is true.
    ///</summary>
    ///<param name="condition">Indicates wether parameter should be added to execution</param>
    ///param name="parameterName">Identificator of parameter on execution</param>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<returns>An instance of builder</returns>
    IExecutionBuilder WithParameter(bool condition, string parameterName, object value);

    ///<summary>
    /// Adds a dynamic object as paramters values by each property will 
    /// transform a parameter to execution, in cases the <paramref name="condition"/> is true.
    /// <example>For Example:
    /// <code>
    /// var dynParam = new { Param1 = "1", Param2 = 2 };
    /// builder.WithParameter(true, dynParam);
    ///</summary>
    ///<param name="condition">Indicates wether parameters should be added to execution</param>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<returns>An instance of builder</returns>
    IExecutionBuilder WithParameter(bool condition, object values);

    ///<summary>
    /// Adds a dynamic object as paramters values by each property will 
    /// transform a parameter to execution
    /// <example>For Example:
    /// <code>
    /// var dynParam = new { Param1 = "1", Param2 = 2 };
    /// builder.WithParameter(dynParam);
    ///</summary>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<returns>An instance of builder</returns>
    IExecutionBuilder WithParameter(object values);

    ///<summary>
    ///Executes a query by all SQL script text and all previous configurations or parameters added.
    ///</summary>
    ///<returns>An enumerable <see cref="dynamic"/> value from query executed</returns>
    IEnumerable<dynamic> Query();

    ///<summary>
    ///Executes a query by all SQL script text and all previous configurations or parameters added.
    ///</summary>
    ///<returns>An enumerable of <typeparamref name="T"/> value from query executed</returns>
    IEnumerable<T> Query<T>();

    ///<summary>
    ///Executes a query by all SQL script text and all previous configurations or parameters added.
    ///</summary>
    ///<param name="cancellation">Allows to cancel all operation by the cancellatio token</param>
    ///<returns>An <see cref="AsyncResult{dynamic}"/> instance to result enumerable values</returns>
    Task<IEnumerable<dynamic>> QueryAsync(CancellationToken cancellation = default);

    ///<summary>
    ///Executes a query by all SQL script text and all previous configurations or parameters added.
    ///</summary>
    ///<param name="cancellation">Allows to cancel all operation by the cancellatio token</param>
    ///<returns>An <see cref="AsyncResult{T}"/>instance to result enumerable values</returns>
    Task<IEnumerable<T>> QueryAsync<T>(CancellationToken cancellation = default);

    ///<summary>
    ///Executes a query by all SQL script text and all previous configurations or parameters added.
    ///The query should result a single result.
    ///</summary>
    ///<returns>A <see cref="dynamic"/> value as single result from query</returns>
    dynamic QuerySingle();

    ///<summary>
    ///Executes a query by all SQL script text and all previous configurations or parameters added.
    ///The query should result a single result.
    ///</summary>
    ///<returns>A <typeparamref name="T"/> value as single result from query</returns>
    T QuerySingle<T>();

    ///<summary>
    ///Executes an awaitable query by all SQL script text and all previous configurations or parameters added.
    ///The query should result a single result.
    ///</summary>
    ///<param name="cancellation">Allows to cancel all operation by the cancellatio token</param>
    ///<returns>A <see cref="dynamic"/> value as single result from query</returns>
    Task<dynamic> QuerySingleAsync(CancellationToken cancellation = default);

    ///<summary>
    ///Executes an awaitable query by all SQL script text and all previous configurations or parameters added.
    ///The query should result a single result.
    ///</summary>
    ///<param name="cancellation">Allows to cancel all operation by the cancellatio token</param>
    ///<returns>A <typeparamref name="T"/> value as single result from query</returns>
    Task<T> QuerySingleAsync<T>(CancellationToken cancellation = default);

    ///<summary>
    ///Executes a query by all SQL script text and all previous configurations or parameters added.
    ///The query should result a single result, otherwise, should returns a default value.
    ///</summary>
    ///<returns>A <see cref="dynamic"/> nullable value as single result from query</returns>
    dynamic? QuerySingleOrDefault();

    ///<summary>
    ///Executes a query by all SQL script text and all previous configurations or parameters added.
    ///The query should result a single result, otherwise, should returns a default value.
    ///</summary>
    ///<returns>A <typeparamref name="T"/> nullable value as single result from query</returns>
    T? QuerySingleOrDefault<T>();

    ///<summary>
    ///Executes an awaitable query by all SQL script text and all previous configurations or parameters added.
    ///The query should result a single result, otherwise, should returns a default value.
    ///</summary>
    ///<param name="cancellation">Allows to cancel all operation by the cancellatio token</param>
    ///<returns>A <see cref="dynamic"/> nullable value as single result from query</returns>
    Task<dynamic?> QuerySingleOrDefaultAsync(CancellationToken cancellation = default);

    ///<summary>
    ///Executes an awaitable query by all SQL script text and all previous configurations or parameters added.
    ///The query should result a single result, otherwise, should returns a default value.
    ///</summary>
    ///<param name="cancellation">Allows to cancel all operation by the cancellatio token</param>
    ///<returns>A <typeparamref name="T"/> nullable value as single result from query</returns>
    Task<T?> QuerySingleOrDefaultAsync<T>(CancellationToken cancellation = default);

    ///<summary>
    ///Executes a query by all SQL script text and all previous configurations or parameters added.
    ///The query can result multiple rows or at least one row, but this operation will result first row.
    ///</summary>
    ///<returns>A <see cref="dynamic"/> value as first result from query</returns>
    dynamic QueryFirst();

    ///<summary>
    ///Executes a query by all SQL script text and all previous configurations or parameters added.
    ///The query can result multiple rows or at least one row, but this operation will result first row.
    ///</summary>
    ///<returns>A <typeparamref name="T"/> value as first result from query</returns>
    T QueryFirst<T>();


    ///<summary>
    ///Executes an awaitable query by all SQL script text and all previous configurations or parameters added.
    ///The query can result multiple rows or at least one row, but this operation will result first row.
    ///</summary>
    ///<param name="cancellation">Allows to cancel all operation by the cancellatio token</param>
    ///<returns>A <see cref="dynamic"/> value as first result from query</returns>
    Task<dynamic> QueryFirstAsync(CancellationToken cancellation = default);

    ///<summary>
    ///Executes an awaitable query by all SQL script text and all previous configurations or parameters added.
    ///The query can result multiple rows or at least one row, but this operation will result first row.
    ///</summary>
    ///<param name="cancellation">Allows to cancel all operation by the cancellatio token</param>
    ///<returns>A <typeparamref name="T"/> value as first result from query</returns>
    Task<T> QueryFirstAsync<T>(CancellationToken cancellation = default);

    ///<summary>
    ///Executes a query by all SQL script text and all previous configurations or parameters added.
    ///The query can result multiple rows, but this operation will result first row. In cases the 
    ///query returns an empty result, it will returns a default value
    ///</summary>
    ///<returns>A <see cref="dynamic"/> value as first result from query</returns>
    dynamic? QueryFirstOrDefault();

    ///<summary>
    ///Executes a query by all SQL script text and all previous configurations or parameters added.
    ///The query can result multiple rows, but this operation will result first row. In cases the 
    ///query returns an empty result, it will returns a default value
    ///</summary>
    ///<returns>A <typeparamref name="T"/> value as first result from query</returns>
    T? QueryFirstOrDefault<T>();

    ///<summary>
    ///Executes an awaitable query by all SQL script text and all previous configurations or parameters added.
    ///The query can result multiple rows, but this operation will result first row. In cases the 
    ///query returns an empty result, it will returns a default value
    ///</summary>
    ///<param name="cancellation">Allows to cancel all operation by the cancellatio token</param>
    ///<returns>A <see cref="dynamic"/> value as first result from query</returns>
    Task<dynamic?> QueryFirstOrDefaultAsync(CancellationToken cancellation = default);

    ///<summary>
    ///Executes an awaitable query by all SQL script text and all previous configurations or parameters added.
    ///The query can result multiple rows, but this operation will result first row. In cases the 
    ///query returns an empty result, it will returns a default value
    ///</summary>
    ///<param name="cancellation">Allows to cancel all operation by the cancellatio token</param>
    ///<returns>A <typeparamref name="T"/> value as first result from query</returns>
    Task<T?> QueryFirstOrDefaultAsync<T>(CancellationToken cancellation = default);

    ///<summary>
    ///Executes a multiple queries by SQL scripts and all previous configurations or parameters added.
    ///The queries can be result multiples rows and all results will merged <typeparamref name="T"/> by a function.
    ///</summary>
    ///<param name="func">An function to merged multiple results from query</param>
    ///<returns>A merged <typeparamref name="T"/> value</returns>
    T QueryMultiple<T>(Func<SqlMapper.GridReader, T> func);

    ///<summary>
    ///Executes an awaitable multiple queries by SQL scripts and all previous configurations or parameters added.
    ///The queries can be result multiples rows and all results will merged <typeparamref name="T"/> by a function.
    ///</summary>
    ///<param name="func">An function to merged multiple results from query</param>
    ///<param name="cancellation">Allows to cancel all operation by the cancellatio token</param>
    ///<returns>A merged <typeparamref name="T"/> value</returns>
    Task<T> QueryMultipleAsync<T>(Func<SqlMapper.GridReader, Task<T>> funcTask, CancellationToken cancellation = default);

    ///<summary>
    ///Executes a SQL scripts that can result a number modifications on execution.
    ///</summary>
    ///<returns>A number of modifications occurred on execution</returns>
    int Execute();

    ///<summary>
    ///Executes an awaitable SQL scripts that can result a number modifications on execution.
    ///</summary>
    ///<param name="cancellation">Allows to cancel all operation by the cancellatio token</param>
    ///<returns>A number of modifications occurred on execution</returns>
    Task<int> ExecuteAsync(CancellationToken cancellation = default);

    ///<summary>
    ///Executes a SQL scripts that will result a object value
    ///</summary>
    ///<returns>An object value from script execution</returns>
    object ExecuteScalar();

    ///<summary>
    ///Executes a SQL scripts that will result a object value
    ///</summary>
    ///<returns>A <typeparamref name="T"/> value from script execution</returns>
    T ExecuteScalar<T>();

    ///<summary>
    ///Executes an awaitable SQL scripts that will result a object value
    ///</summary>
    ///<returns>An object value from script execution</returns>
    Task<object> ExecuteScalarAsync(CancellationToken cancellation = default);

    ///<summary>
    ///Executes an awaitable SQL scripts that will result a object value
    ///</summary>
    ///<param name="cancellation">Allows to cancel all operation by the cancellatio token</param>
    ///<returns>A <typeparamref name="T"/> value from script execution</returns>
    Task<T> ExecuteScalarAsync<T>(CancellationToken cancellation = default);

    ///<summary>
    ///Executes a SQL script and results a way to iterates all rows from script
    ///</summary>
    ///<returns>A <typeparamref name="T"/> value from script execution</returns>
    IDataReader ExecuteDataReader();

    ///<summary>
    ///Executes an awaitable SQL script and results a way to iterates all rows from script
    ///</summary>
    ///<param name="cancellation">Allows to cancel all operation by the cancellatio token</param>
    ///<returns>A <typeparamref name="T"/> value from script execution</returns>
    Task<IDataReader> ExecuteDataReaderAsync(CancellationToken cancellation = default);
}
