using System.Data;
using Dapper.FluentExecution.Abstractions;
using Dapper.FluentExecution.Tests.Models;

namespace Dapper.FluentExecution.Tests.ExecutionSqlBuilderTests;

[Trait(nameof(ExecutionSqlBuilder), nameof(IExecutionBuilder.Execute))]
[Trait(nameof(ExecutionSqlBuilder), nameof(IExecutionBuilder.ExecuteScalar))]
public class ExecuteUnitTests : ExecutionSqlBuilderTestBase
{
    public ExecuteUnitTests() : base() { }

    [Theory]
    [MemberData(nameof(ExecuteActionsData.GenerateActions), MemberType = typeof(ExecuteActionsData))]
    public void When_Performs_All_Executes_On_Dapper_Should_Dispose_Builder_And_Grab_Result(Func<IExecutionBuilder, object> dapperFunc, Action<object> assertAct)
    {
        //We just want test if all the dapper functions is
        //calling the dispose of builder after get the result
        //and the result should be retrivied by connection/command
        //with dapper specific format
        //arrange
        string sql = "select * from table";
        command.SetupDataReader();

        //act
        var builder = sql
            .On(Connection);

        var result = dapperFunc(builder);

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

public class ExecuteActionsData
{
    public static IEnumerable<object[]> GenerateActions()
    {
        yield return new object[] { (IExecutionBuilder e) => (object)e.Execute(), (object o) => { o.As<int>().Should().Be(1); } };
        yield return new object[] { (IExecutionBuilder e) => e.ExecuteScalar(), (object o) => { o.As<object>().Should().NotBeNull(); } };
        yield return new object[] { (IExecutionBuilder e) => e.ExecuteScalar<Person>(), (object o) => { o.As<Person>().Should().NotBeNull().And.Match<Person>(p => p.Name == "Ant"); } };
    }
}

