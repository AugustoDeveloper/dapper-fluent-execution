using System.Data;
using Dapper.FluentExecution.Tests.Fakes;

namespace Dapper.FluentExecution.Tests.ExecutionSqlBuilderTests;

public abstract class ExecutionSqlBuilderTestBase
{
    protected readonly Mock<IDbConnection> mockConnection;
    internal readonly FakeDbCommand command;

    public IDbConnection Connection => mockConnection.Object;

    protected ExecutionSqlBuilderTestBase()
    {
        mockConnection = new(MockBehavior.Strict);
        command = new();

        mockConnection
            .Setup(c => c.ConnectionString)
            .Returns("Data Source=127.0.0.1,1433;TrustServerCertificate=true;Initial Catalog=db;Persist Security Info=True;User ID=sa; password=123;MultipleActiveResultSets=True;");

        mockConnection
            .Setup(c => c.State)
            .Returns(ConnectionState.Open);

        mockConnection
            .Setup(c => c.CreateCommand())
            .Returns(command);
    }
}

public record ParameterItem
{
    public bool Condition { get; init; }
    public string ParameterName { get; init; } = null!;
    public DbType DbType { get; init; }
    public object Value { get; init; } = null!;
    public int? Size { get; init; }
}

public class ParametersData
{
    public static IEnumerable<object[]> GenerateParametersWithDefaultTypesAndRandomCondition()
    {
        yield return new object[]
        {
            new ParameterItem { Condition = true, ParameterName = "param", Value = 'c', DbType = DbType.StringFixedLength },
            new ParameterItem { Condition = false, ParameterName = "param1", Value = true, DbType = DbType.Boolean },
            new ParameterItem { Condition = true, ParameterName = "param2", Value = (byte)1, DbType = DbType.Byte },
        };

        yield return new object[]
        {
            new ParameterItem { Condition = true, ParameterName = "param", Value = (short)1, DbType = DbType.Int16 },
            new ParameterItem { Condition = true, ParameterName = "param1", Value = 1, DbType = DbType.Int32 },
            new ParameterItem { Condition = false, ParameterName = "param2", Value = (long)1, DbType = DbType.Int64 },
        };
        yield return new object[]
        {
            new ParameterItem { Condition = true, ParameterName = "param", Value = 10.5, DbType = DbType.Double },
            new ParameterItem { Condition = true, ParameterName = "param1", Value = 10.5f, DbType = DbType.Single },
            new ParameterItem { Condition = true, ParameterName = "param2", Value = 10.5M, DbType = DbType.Decimal },
        };
        yield return new object[]
        {
            new ParameterItem { Condition = false, ParameterName = "param", Value = "ZenBrain", DbType = DbType.String, Size = 8 },
            new ParameterItem { Condition = false, ParameterName = "param1", Value = DateTime.Now, DbType = DbType.DateTime },
            new ParameterItem { Condition = true, ParameterName = "param2", Value = DateTimeOffset.Now, DbType = DbType.DateTimeOffset }
        };
    }

    public static IEnumerable<object[]> GenerateParameterItems()
    {
        yield return new object[]
        {
            new ParameterItem { Condition = true, ParameterName = "param", Value = 'c', DbType = DbType.StringFixedLength },
            new ParameterItem { Condition = true, ParameterName = "param1", Value = true, DbType = DbType.Boolean },
            new ParameterItem { Condition = true, ParameterName = "param2", Value = (byte)1, DbType = DbType.Byte },
        };

        yield return new object[]
        {
            new ParameterItem { Condition = false, ParameterName = "param", Value = (short)1, DbType = DbType.Int16 },
            new ParameterItem { Condition = false, ParameterName = "param1", Value = 1, DbType = DbType.Int32 },
            new ParameterItem { Condition = true, ParameterName = "param2", Value = (long)1, DbType = DbType.Int64 },
        };
        yield return new object[]
        {
            new ParameterItem { Condition = true, ParameterName = "param", Value = 10.5, DbType = DbType.Double },
            new ParameterItem { Condition = false, ParameterName = "param1", Value = 10.5f, DbType = DbType.Single },
            new ParameterItem { Condition = false, ParameterName = "param2", Value = 10.5M, DbType = DbType.Decimal },
        };
        yield return new object[]
        {
            new ParameterItem {Condition = true, ParameterName = "param", Value = "ZenBrain", DbType = DbType.String },
            new ParameterItem {Condition = true, ParameterName = "param1", Value = DateTime.Now, DbType = DbType.AnsiString },
            new ParameterItem {Condition = true, ParameterName = "param2", Value = TimeSpan.FromMinutes(2), DbType = DbType.AnsiString },
        };
    }
}
