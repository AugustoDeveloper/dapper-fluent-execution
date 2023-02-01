using System.Data;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution.Tests.ExecutionSqlBuilderTests;

[Trait(nameof(ExecutionSqlBuilder), nameof(IExecutionBuilder.AsStoredProcedure))]
public class AsStoredProcedureUnitTests : ExecutionSqlBuilderTestBase
{
    public AsStoredProcedureUnitTests() : base() { }

    [Fact]
    public void When_Try_To_Query_As_Stored_Procedure_Should_Change_CommandType_To_StoredProcedure()
    {
        //arrange
        string sql = "select * from table";
        command.SetupDataReader();

        //act
        var result = sql
            .On(Connection)
            .AsStoredProcedure()
            .Query();

        //assert
        command.CommandText
            .Should()
            .Be(sql);

        command.CommandType
            .Should()
            .Be(CommandType.StoredProcedure);

        command.FakeParameters.Should().BeEmpty();
    }
}
