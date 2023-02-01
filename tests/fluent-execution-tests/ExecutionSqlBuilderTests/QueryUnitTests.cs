using System.Data;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution.Tests.ExecutionSqlBuilderTests;

[Trait(nameof(ExecutionSqlBuilder), nameof(IExecutionBuilder.Query))]
public class QueryUnitTests : ExecutionSqlBuilderTestBase
{
    public QueryUnitTests() : base() { }

    [Theory]
    [MemberData(nameof(QueryActionsData.GenerateActions), MemberType = typeof(QueryActionsData))]
    public void When_Query_Without_Parameters_Should_Should_Returns_Enumerable_Of_Dynamics(Func<IExecutionBuilder, object> dapperFunc, Action<object> assertAct)
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

public class QueryActionsData
{
    public static IEnumerable<object[]> GenerateActions()
    {
        yield return new object[] { (IExecutionBuilder e) => e.Query(), (object o) => { o.As<IEnumerable<dynamic>>().Should().HaveCount(1); } };
        yield return new object[] { (IExecutionBuilder e) => e.Query<string>(), (object o) => { o.As<IEnumerable<string>>().Should().HaveCount(1); } };
        yield return new object[] { (IExecutionBuilder e) => e.QueryFirst(), (object o) => { o.Should().NotBeNull(); } };
        yield return new object[] { (IExecutionBuilder e) => e.QueryFirst<string>(), (object o) => { o.Should().Be("Value"); } };
        yield return new object[] { (IExecutionBuilder e) => e.QuerySingle(), (object o) => { o.Should().NotBeNull(); } };
        yield return new object[] { (IExecutionBuilder e) => e.QuerySingle<string>(), (object o) => { o.Should().Be("Value"); } };
        yield return new object[] { (IExecutionBuilder e) => e.QueryFirstOrDefault(), (object o) => { o.Should().NotBeNull(); } };
        yield return new object[] { (IExecutionBuilder e) => e.QueryFirstOrDefault<string>(), (object o) => { o.Should().Be("Value"); } };
        yield return new object[] { (IExecutionBuilder e) => e.QuerySingleOrDefault(), (object o) => { o.Should().NotBeNull(); } };
        yield return new object[] { (IExecutionBuilder e) => e.QuerySingleOrDefault<string>(), (object o) => { o.Should().Be("Value"); } };
        yield return new object[] { (IExecutionBuilder e) => e.ExecuteDataReader(), (object o) => { o.Should().NotBeNull(); } };
    }
}
