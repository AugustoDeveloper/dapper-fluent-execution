using System.Data;
using System.Data.Common;
using Dapper.FluentExecution.Tests.Models;

namespace Dapper.FluentExecution.Tests.Fakes;

internal class FakeDbCommand : DbCommand
{
    internal FakeParameterCollection FakeParameters { get; }
    public override UpdateRowSource UpdatedRowSource { get; set; }
    protected override DbTransaction? DbTransaction { get; set; }
    public override string CommandText { get; set; } = null!;
    public override CommandType CommandType { get; set; }
    public override int CommandTimeout { get; set; }
    public override bool DesignTimeVisible { get; set; }
    protected override DbConnection? DbConnection { get; set; }
    protected override DbParameterCollection DbParameterCollection => FakeParameters;

    internal DbDataReader? DataReader { get; set; }
    internal Stack<DbDataReader> MultipleDataReaders { get; } = new();

    public FakeDbCommand() : base()
    {
        FakeParameters = new();
    }
    internal void SetupDataReader()
    {
        var table = new DataTable();

        var column = table.Columns.Add();
        column.ColumnName = "Column";

        var row = table.NewRow();
        row["Column"] = "Value";

        table.Rows.Add(row);
        DataReader = table.CreateDataReader();
    }

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        if (DataReader is null)
            Console.WriteLine("Reading multiple");
        return DataReader ?? MultipleDataReaders.Pop();
    }

    protected override DbParameter CreateDbParameter()
    {
        return new FakeParameter();
    }

    public override int ExecuteNonQuery()
    {
        return 1;
    }

    public override object? ExecuteScalar()
    {
        return new Person { Name = "Ant" };
    }

    public override void Prepare()
    {
        throw new NotImplementedException();
    }

    public override void Cancel()
    {
        throw new NotImplementedException();
    }
}
