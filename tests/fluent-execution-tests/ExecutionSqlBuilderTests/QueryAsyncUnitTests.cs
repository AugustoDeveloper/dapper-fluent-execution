using System.Data;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution.Tests.ExecutionSqlBuilderTests;

[Trait(nameof(ExecutionSqlBuilder), nameof(IExecutionBuilder.Query))]
public class QueryAsyncUnitTests : ExecutionSqlBuilderTestBase
{
    public QueryAsyncUnitTests() : base() { }

    [Theory]
    [MemberData(nameof(QueryAsyncActionsData.GenerateActions), MemberType = typeof(QueryAsyncActionsData))]
    public async Task When_QueryAsync_Without_Parameters_Should_Should_Returns_Enumerable_Of_Dynamics(Func<IExecutionBuilder, Task<object>> dapperFunc, Action<object> assertAct)
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

public class QueryAsyncActionsData
{
    public static IEnumerable<object[]> GenerateActions()
    {
        yield return new object[] { async (IExecutionBuilder e) => { var result = await e.QueryAsync(); return (object)result; }, (object o) => { o.As<IEnumerable<dynamic>>().Should().HaveCount(1); } };
        yield return new object[] { async (IExecutionBuilder e) => { var result = await e.QueryAsync<string>(); return (object)result; }, (object o) => { o.As<IEnumerable<string>>().Should().HaveCount(1); } };
        yield return new object[] { async (IExecutionBuilder e) => { var result = await e.QueryFirstAsync(); return (object)result; }, (object o) => { o.Should().NotBeNull(); } };
        yield return new object[] { async (IExecutionBuilder e) => { var result = await e.QueryFirstAsync<string>(); return (object)result; }, (object o) => { o.Should().Be("Value"); } };
        yield return new object[] { async (IExecutionBuilder e) => { var result = await e.QuerySingleAsync(); return (object)result; }, (object o) => { o.Should().NotBeNull(); } };
        yield return new object[] { async (IExecutionBuilder e) => { var result = await e.QuerySingleAsync<string>(); return (object)result; }, (object o) => { o.Should().Be("Value"); } };
        yield return new object[] { async (IExecutionBuilder e) => { var result = await e.QueryFirstOrDefaultAsync(); return (object)result!; }, (object o) => { o.Should().NotBeNull(); } };
        yield return new object[] { async (IExecutionBuilder e) => { var result = await e.QueryFirstOrDefaultAsync<string>(); return (object)result!; }, (object o) => { o.Should().Be("Value"); } };
        yield return new object[] { async (IExecutionBuilder e) => { var result = await e.QuerySingleOrDefaultAsync(); return (object)result!; }, (object o) => { o.Should().NotBeNull(); } };
        yield return new object[] { async (IExecutionBuilder e) => { var result = await e.QuerySingleOrDefaultAsync<string>(); return (object)result!; }, (object o) => { o.Should().Be("Value"); } };
        yield return new object[] { async (IExecutionBuilder e) => { var result = await e.ExecuteDataReaderAsync(); return (object)result!; }, (object o) => { o.Should().NotBeNull(); } };
    }
}
