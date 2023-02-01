using System.Data;
using Dapper.FluentExecution.Abstractions;
using Dapper.FluentExecution.Tests.Models;

namespace Dapper.FluentExecution.Tests.ExecutionSqlBuilderTests;

[Trait(nameof(ExecutionSqlBuilder), nameof(IExecutionBuilder.ExecuteAsync))]
[Trait(nameof(ExecutionSqlBuilder), nameof(IExecutionBuilder.ExecuteScalarAsync))]
public class ExecuteAsyncUnitTests : ExecutionSqlBuilderTestBase
{
    public ExecuteAsyncUnitTests() : base() { }

    [Theory]
    [MemberData(nameof(ExecuteAsyncActionsData.GenerateActions), MemberType = typeof(ExecuteAsyncActionsData))]
    public async Task When_Performs_All_Executes_On_Dapper_Should_Dispose_Builder_And_Grab_Result(Func<IExecutionBuilder, Task<object>> dapperFunc, Action<object> assertAct)
    {
        //We just want test if all the dapper functions is
        //calling the dispose of builder after get the result
        //and the result should be retrivied by connection/command
        //with dapper specific format
        //arrange
        string sql = "select * from table";

        //act
        var builder = sql
            .On(Connection);

        var result = await dapperFunc(builder);

        //assert
        command.CommandText
            .Should()
            .Be(sql);

        command.CommandType
            .Should()
            .Be(CommandType.Text);

        command.FakeParameters.Should().BeEmpty();

        builder.Disposed.Should().BeTrue();
        assertAct(result);
    }
}

public class ExecuteAsyncActionsData
{
    public static IEnumerable<object[]> GenerateActions()
    {
        yield return new object[] { async (IExecutionBuilder e) => (object)await e.ExecuteAsync(), (object o) => { o.As<int>().Should().Be(1); } };
        yield return new object[] { async (IExecutionBuilder e) => (object)await e.ExecuteScalarAsync(), (object o) => { o.As<object>().Should().NotBeNull(); } };
        yield return new object[] { async (IExecutionBuilder e) => (object)await e.ExecuteScalarAsync<Person>(), (object o) => { o.As<Person>().Should().NotBeNull().And.Match<Person>(p => p.Name == "Ant"); } };
    }
}
