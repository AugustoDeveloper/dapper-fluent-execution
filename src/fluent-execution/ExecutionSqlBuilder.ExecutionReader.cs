using System.Data;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution;

internal partial class ExecutionSqlBuilder : IExecutionBuilder
{
    IDataReader IExecutionBuilder.ExecuteDataReader()
        => PrepareAndExecute(this.connection.ExecuteReader);
}
