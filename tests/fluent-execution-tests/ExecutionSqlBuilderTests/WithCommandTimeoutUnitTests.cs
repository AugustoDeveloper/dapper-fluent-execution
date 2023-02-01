using System.Data;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution.Tests.ExecutionSqlBuilderTests;

[Trait(nameof(ExecutionSqlBuilder), nameof(IExecutionBuilder.WithCommandTimeout))]
public class WithCommandTimeoutUnitTests : ExecutionSqlBuilderTestBase
{
    public WithCommandTimeoutUnitTests() : base() { }

    [Fact]
    public void When_Query_Without_Parameters_Should_Should_Returns_Enumerable_Of_Dynamics()
    {
        //arrange
        string sql = "select * from table";
        command.SetupDataReader();

        //act
        var result = sql
            .On(Connection)
            .WithCommandTimeout(TimeSpan.FromMinutes(5))
            .Query();

        //assert
        command.CommandText
            .Should()
            .Be(sql);

        command.CommandType
            .Should()
            .Be(CommandType.Text);

        command.FakeParameters.Should().BeEmpty();
        command.CommandTimeout.Should().Be(300);
    }
}
