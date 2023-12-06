using System.Data;

namespace Dapper.FluentExecution.Abstractions;

///<summary>
///Defines a protocol to produces parameters
///to support sql script
///</summary>
public interface IParameterActionBuilder
{
    ///<summary>
    ///Adds a parameter of specific type and size indicating
    ///</summary>
    ///<param name="parameterName">Identificator of parameter on execution</param>
    ///<param name="dbType">Database type of parameter</param>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<param name="size">Size of value on database</param>
    ///<returns>An instance of builder</returns>
    IParameterActionBuilder WithParameter(string parameterName, DbType dbType, object value, int size);

    ///<summary>
    ///Adds a parameter of specific type and size indicating, in cases the condition is true.
    ///</summary>
    ///<param name="condition">Indicates wether parameter should be added to execution</param>
    ///<param name="parameterName">Identificator of parameter on execution</param>
    ///<param name="dbType">Database type of parameter</param>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<param name="size">Size of value on database</param>
    ///<returns>An instance of builder</returns>
    IParameterActionBuilder WithParameter(bool condition, string parameterName, DbType dbType, object value, int size);

    ///<summary>
    ///Adds a parameter of specific type
    ///</summary>
    ///<param name="parameterName">Identificator of parameter on execution</param>
    ///<param name="dbType">Database type of parameter</param>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<returns>An instance of builder</returns>
    IParameterActionBuilder WithParameter(string parameterName, DbType dbType, object value);

    ///<summary>
    ///Adds a parameter of specific type in cases the condition is true.
    ///</summary>
    ///<param name="condition">Indicates wether parameter should be added to execution</param>
    ///<param name="parameterName">Identificator of parameter on execution</param>
    ///<param name="dbType">Database type of parameter</param>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<returns>An instance of builder</returns>
    IParameterActionBuilder WithParameter(bool condition, string parameterName, DbType dbType, object value);

    ///<summary>
    ///Adds a parameter with <paramref name="parameterName"/> identificator and
    ///with stored value on <paramref name="value"/>.
    ///</summary>
    ///<param name="parameterName">Identificator of parameter on execution</param>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<returns>An instance of builder</returns>
    IParameterActionBuilder WithParameter(string parameterName, object value);

    ///<summary>
    ///Adds a parameter with <paramref name="parameterName"/> identificator and
    ///with stored value on <paramref name="value"/>, in cases the <paramref name="condition"/> 
    ///is true.
    ///</summary>
    ///<param name="condition">Indicates wether parameter should be added to execution</param>
    ///param name="parameterName">Identificator of parameter on execution</param>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<returns>An instance of builder</returns>
    IParameterActionBuilder WithParameter(bool condition, string parameterName, object value);

    ///<summary>
    /// Adds a dynamic object as paramters values by each property will 
    /// transform a parameter to execution, in cases the <paramref name="condition"/> is true.
    /// <example>For Example:
    /// <code>
    /// var dynParam = new { Param1 = "1", Param2 = 2 };
    /// builder.WithParameter(true, dynParam);
    ///</summary>
    ///<param name="condition">Indicates wether parameters should be added to execution</param>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<returns>An instance of builder</returns>
    IParameterActionBuilder WithParameter(bool condition, object values);

    ///<summary>
    /// Adds a dynamic object as paramters values by each property will 
    /// transform a parameter to execution
    /// <example>For Example:
    /// <code>
    /// var dynParam = new { Param1 = "1", Param2 = 2 };
    /// builder.WithParameter(dynParam);
    ///</summary>
    ///<param name="values">Object value to transform each property in parameters</param>
    ///<returns>An instance of builder</returns>
    IParameterActionBuilder WithParameter(object values);
}

