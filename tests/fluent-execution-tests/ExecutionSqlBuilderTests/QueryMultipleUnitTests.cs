using System.Data;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution.Tests.ExecutionSqlBuilderTests;

[Trait(nameof(ExecutionSqlBuilder), nameof(IExecutionBuilder.QueryMultiple))]
public class QueryMultipleUnitTests : ExecutionSqlBuilderTestBase
{
    public QueryMultipleUnitTests() : base() { }

    [Fact]
    public void When()
    {
        //arrange
        string sql = "select * from table; select * from table2";
        command.SetupDataReader();

        //act
        var builder = sql
            .On(Connection);

        var result = builder
            .QueryMultiple((grid) => grid.Read<string>())
            .ToList();



        //assert
        result.Should().NotBeNull().And.NotBeEmpty();

        command.CommandText
            .Should()
                .Be(sql);

        command.CommandType
            .Should()
                .Be(CommandType.Text);

        command.FakeParameters.Should().BeEmpty();

        builder.Disposed.Should().BeTrue();
    }
}
