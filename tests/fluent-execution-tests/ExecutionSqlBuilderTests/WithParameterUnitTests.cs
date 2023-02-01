using System.Data;
using Dapper.FluentExecution.Abstractions;

namespace Dapper.FluentExecution.Tests.ExecutionSqlBuilderTests;

[Trait(nameof(ExecutionSqlBuilder), nameof(IExecutionBuilder.WithParameter))]
public class WithParameterUnitTests : ExecutionSqlBuilderTestBase
{
    public WithParameterUnitTests() : base() { }

    [Theory]
    [MemberData(nameof(ParametersData.GenerateParametersWithDefaultTypesAndRandomCondition), MemberType = typeof(ParametersData))]
    public void When_Add_With_Specified_Name_Value_DbType_And_Size_Parameters_At_Condition_Should_Stored_Parameters_Only_Condition_Is_True(ParameterItem param, ParameterItem param1, ParameterItem param2)
    {
        //arrange
        string sql = $"select * from table where {param.ParameterName} = @{param.ParameterName} and {param1.ParameterName} = @{param1.ParameterName} and {param2.ParameterName} = @{param2.ParameterName}";

        command.SetupDataReader();

        //act
        var result = sql
            .On(Connection)
            .WithParameter(param.Condition, param.ParameterName, param.DbType, param.Value, param.Size.GetValueOrDefault(0))
            .WithParameter(param1.Condition, param1.ParameterName, param1.DbType, param1.Value, param1.Size.GetValueOrDefault(0))
            .WithParameter(param2.Condition, param2.ParameterName, param2.DbType, param2.Value, param2.Size.GetValueOrDefault(0))
            .Query();

        var count = Convert.ToInt32(param.Condition) +
            Convert.ToInt32(param1.Condition) +
            Convert.ToInt32(param2.Condition);

        //assert
        command.CommandText
            .Should()
            .Be(sql);

        command.CommandType
            .Should()
            .Be(CommandType.Text);

        command.FakeParameters
            .Should()
            .HaveCount(count);

    }

    [Theory]
    [MemberData(nameof(ParametersData.GenerateParametersWithDefaultTypesAndRandomCondition), MemberType = typeof(ParametersData))]
    public void When_Add_With_Specified_Name_Value_DbType_And_Size_Parameters_At_Condtion_Should_Executed_With_Parameter_Passed(ParameterItem param, ParameterItem param1, ParameterItem param2)
    {
        //arrange
        string sql = $"select * from table where {param.ParameterName} = @{param.ParameterName} and {param1.ParameterName} = @{param1.ParameterName} and {param2.ParameterName} = @{param2.ParameterName}";
        command.SetupDataReader();

        //act
        var result = sql
            .On(Connection)
            .WithParameter(param.Condition, param.ParameterName, param.DbType, param.Value, param.Size.GetValueOrDefault(0))
            .WithParameter(param1.Condition, param1.ParameterName, param1.DbType, param1.Value, param1.Size.GetValueOrDefault(0))
            .WithParameter(param2.Condition, param2.ParameterName, param2.DbType, param2.Value, param2.Size.GetValueOrDefault(0))
            .Query();

        var count = Convert.ToInt32(param.Condition) +
            Convert.ToInt32(param1.Condition) +
            Convert.ToInt32(param2.Condition);

        //assert
        command.CommandText
            .Should()
            .Be(sql);

        command.CommandType
            .Should()
            .Be(CommandType.Text);

        command.FakeParameters
            .Should()
            .HaveCount(count);
    }

    [Theory]
    [MemberData(nameof(ParametersData.GenerateParametersWithDefaultTypesAndRandomCondition), MemberType = typeof(ParametersData))]
    public void When_Add_With_Specified_Name_Value_DbType_And_Size_Parameters_Should_Executed_With_Parameter_Passed(ParameterItem param, ParameterItem param1, ParameterItem param2)
    {
        //arrange
        string sql = $"select * from table where {param.ParameterName} = @{param.ParameterName} and {param1.ParameterName} = @{param1.ParameterName} and {param2.ParameterName} = @{param2.ParameterName}";
        command.SetupDataReader();

        //act
        var result = sql
            .On(Connection)
            .WithParameter(param.ParameterName, param.DbType, param.Value, param.Size.GetValueOrDefault(0))
            .WithParameter(param1.ParameterName, param1.DbType, param1.Value, param1.Size.GetValueOrDefault(0))
            .WithParameter(param2.ParameterName, param2.DbType, param2.Value, param2.Size.GetValueOrDefault(0))
            .Query();

        //assert
        command.CommandText
            .Should()
            .Be(sql);

        command.CommandType
            .Should()
            .Be(CommandType.Text);

        command.FakeParameters
            .Should()
            .NotBeEmpty()
            .And
            .HaveCount(3)
            .And
            .Contain(p => p.As<IDbDataParameter>().ParameterName == param.ParameterName &&
                    p.As<IDbDataParameter>().Direction == ParameterDirection.Input &&
                    p.As<IDbDataParameter>().DbType == param.DbType &&
                    p.As<IDbDataParameter>().Size == param.Size.GetValueOrDefault(0) &&
                    p.As<IDbDataParameter>().Value == param.Value)
            .And
            .Contain(p => p.As<IDbDataParameter>().ParameterName == param1.ParameterName &&
                    p.As<IDbDataParameter>().Direction == ParameterDirection.Input &&
                    p.As<IDbDataParameter>().Size == param1.Size.GetValueOrDefault(0) &&
                    p.As<IDbDataParameter>().DbType == param1.DbType &&
                    p.As<IDbDataParameter>().Value == param1.Value)
            .And
            .Contain(p => p.As<IDbDataParameter>().ParameterName == param2.ParameterName &&
                    p.As<IDbDataParameter>().Direction == ParameterDirection.Input &&
                    p.As<IDbDataParameter>().Size == param2.Size.GetValueOrDefault(0) &&
                    p.As<IDbDataParameter>().DbType == param2.DbType &&
                    p.As<IDbDataParameter>().Value == param2.Value);
    }

    [Theory]
    [MemberData(nameof(ParametersData.GenerateParametersWithDefaultTypesAndRandomCondition), MemberType = typeof(ParametersData))]
    public void When_Add_With_Specified_Name_Value_And_DbType_Parameters_Should_Executed_With_Parameter_Passed(ParameterItem param, ParameterItem param1, ParameterItem param2)
    {
        //arrange
        string sql = $"select * from table where {param.ParameterName} = @{param.ParameterName} and {param1.ParameterName} = @{param1.ParameterName} and {param2.ParameterName} = @{param2.ParameterName}";
        command.SetupDataReader();

        //act
        var result = sql
            .On(Connection)
            .WithParameter(param.ParameterName, param.DbType, param.Value)
            .WithParameter(param1.ParameterName, param1.DbType, param1.Value)
            .WithParameter(param2.ParameterName, param2.DbType, param2.Value)
            .Query();

        //assert
        command.CommandText
            .Should()
            .Be(sql);

        command.CommandType
            .Should()
            .Be(CommandType.Text);

        command.FakeParameters
            .Should()
            .NotBeEmpty()
            .And
            .HaveCount(3)
            .And
            .Contain(p => p.As<IDbDataParameter>().ParameterName == param.ParameterName &&
                    p.As<IDbDataParameter>().Direction == ParameterDirection.Input &&
                    p.As<IDbDataParameter>().DbType == param.DbType &&
                    p.As<IDbDataParameter>().Value == param.Value)
            .And
            .Contain(p => p.As<IDbDataParameter>().ParameterName == param1.ParameterName &&
                    p.As<IDbDataParameter>().Direction == ParameterDirection.Input &&
                    p.As<IDbDataParameter>().DbType == param1.DbType &&
                    p.As<IDbDataParameter>().Value == param1.Value)
            .And
            .Contain(p => p.As<IDbDataParameter>().ParameterName == param2.ParameterName &&
                    p.As<IDbDataParameter>().Direction == ParameterDirection.Input &&
                    p.As<IDbDataParameter>().DbType == param2.DbType &&
                    p.As<IDbDataParameter>().Value == param2.Value);
    }

    [Theory]
    [MemberData(nameof(ParametersData.GenerateParameterItems), MemberType = typeof(ParametersData))]
    public void When_Add_With_Specified_Name_And_Value_Parameters_Should_Executed_With_Parameter_Passed(ParameterItem param, ParameterItem param1, ParameterItem param2)
    {
        //arrange
        string sql = $"select * from table where {param.ParameterName} = @{param.ParameterName} and {param1.ParameterName} = @{param1.ParameterName} and {param2.ParameterName} = @{param2.ParameterName}";
        command.SetupDataReader();

        //act
        var result = sql
            .On(Connection)
            .WithParameter(param.ParameterName, param.Value)
            .WithParameter(param1.ParameterName, param1.Value)
            .WithParameter(param2.ParameterName, param2.Value)
            .Query();

        //assert
        command.CommandText
            .Should()
            .Be(sql);

        command.CommandType
            .Should()
            .Be(CommandType.Text);

        command.FakeParameters
            .Should()
            .NotBeEmpty()
            .And
            .HaveCount(3)
            .And
            .Contain(p => p.As<IDbDataParameter>().ParameterName == param.ParameterName &&
                    p.As<IDbDataParameter>().Direction == ParameterDirection.Input &&
                    p.As<IDbDataParameter>().DbType == param.DbType &&
                    p.As<IDbDataParameter>().Value == param.Value)
            .And
            .Contain(p => p.As<IDbDataParameter>().ParameterName == param1.ParameterName &&
                    p.As<IDbDataParameter>().Direction == ParameterDirection.Input &&
                    p.As<IDbDataParameter>().DbType == param1.DbType &&
                    p.As<IDbDataParameter>().Value == param1.Value)
            .And
            .Contain(p => p.As<IDbDataParameter>().ParameterName == param2.ParameterName &&
                    p.As<IDbDataParameter>().Direction == ParameterDirection.Input &&
                    p.As<IDbDataParameter>().DbType == param2.DbType &&
                    p.As<IDbDataParameter>().Value == param2.Value);
    }

    [Theory]
    [MemberData(nameof(ParametersData.GenerateParameterItems), MemberType = typeof(ParametersData))]
    public void When_Add_With_Dynamic_Object_Parameters_Should_Executed_With_Parameter_Passed(ParameterItem param, ParameterItem param1, ParameterItem param2)
    {
        //arrange
        string sql = $"select * from table where {param.ParameterName} = @{param.ParameterName} and {param1.ParameterName} = @{param1.ParameterName} and {param2.ParameterName} = @{param2.ParameterName}";
        command.SetupDataReader();

        //act
        var result = sql
            .On(Connection)
            .WithParameter(new { param = param.Value, param1 = param1.Value, param2 = param2.Value })
            .Query();

        //assert
        command.CommandText
            .Should()
            .Be(sql);

        command.CommandType
            .Should()
            .Be(CommandType.Text);

        command.FakeParameters
            .Should()
            .NotBeEmpty()
            .And
            .HaveCount(3)
            .And
            .Contain(p => p.As<IDbDataParameter>().ParameterName == param.ParameterName &&
                    p.As<IDbDataParameter>().Direction == ParameterDirection.Input &&
                    p.As<IDbDataParameter>().DbType == param.DbType &&
                    p.As<IDbDataParameter>().Value == param.Value)
            .And
            .Contain(p => p.As<IDbDataParameter>().ParameterName == param1.ParameterName &&
                    p.As<IDbDataParameter>().Direction == ParameterDirection.Input &&
                    p.As<IDbDataParameter>().DbType == param1.DbType &&
                    p.As<IDbDataParameter>().Value == param1.Value)
            .And
            .Contain(p => p.As<IDbDataParameter>().ParameterName == param2.ParameterName &&
                    p.As<IDbDataParameter>().Direction == ParameterDirection.Input &&
                    p.As<IDbDataParameter>().DbType == param2.DbType &&
                    p.As<IDbDataParameter>().Value == param2.Value);
    }
}
