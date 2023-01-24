using System.Data;

namespace Dapper.FluentExecution.Tests.StringExtesionsTests;

public class OnUniTests
{
    private readonly Mock<IDbConnection> mockConnection;
    public IDbConnection Connection => mockConnection.Object;

    public OnUniTests()
    {
        mockConnection = new(MockBehavior.Strict);
    }

    private delegate void RequestDelegate();

    [Fact]
    public void When_Pass_Valid_Connection_Should_Returns_New_Instance()
    {
        //arrange
        string? sql = "SELECT * FROM Persons";

        //act | assert
        sql
            .On(Connection).Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void When_Pass_Invalid_Sql_Should_Thrown_ArgumentNullException(string sql)
    {
        //arrange
        //act | assert
        FluentActions
            .Invoking(() => sql.On(Connection))
            .Should()
            .ThrowExactly<ArgumentException>()
            .WithParameterName(nameof(sql));
    }

    [Fact]
    public void When_Pass_Null_Connection_Should_Thrown_ArgumentNullException()
    {
        //arrange
        string? sql = "SELECT * FROM Persons";

        //act | assert
        FluentActions
            .Invoking(() => sql.On(null))
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithParameterName("connection");
    }
}
