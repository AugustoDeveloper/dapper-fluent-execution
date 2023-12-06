namespace Dapper.FluentExecution.Abstractions;

///<summary>
///Defines a protocol to creates parameters
///support sql script execution
///</summary>
public interface IParameterableBuilder : IParameterActionBuilder, IDisposable
{
    DynamicParameters Build();
}
