using System.Collections;
using System.Data;
using System.Data.Common;

namespace Dapper.FluentExecution.Tests.Fakes;

public class FakeParameterCollection : DbParameterCollection, IList, IEnumerable<DbParameter>
{
    private readonly List<DbParameter> parameters = new();

    public override int Count => parameters.Count;
    public override object SyncRoot => parameters;

    public FakeParameterCollection() : base() { }

    public override void AddRange(Array values)
        => parameters.AddRange(values.Cast<DbParameter>());

    public override void Clear()
    {
        // We need override the clear function,
        // cause the dapper clear all parameters
        // after execution script and we need to check
        //base.Clear();
    }

    public override int Add(object value)
    {
        parameters.Add((DbParameter)value);
        return Count;
    }

    public override bool Contains(object value)
        => parameters.Contains(value);

    public override bool Contains(string value)
        => IndexOf(value) > -1;

    public override void CopyTo(Array array, int index) { }

    public override IEnumerator GetEnumerator()
        => parameters.GetEnumerator();

    public override int IndexOf(object value)
        => parameters.IndexOf((DbParameter)value);

    public override int IndexOf(string parameterName)
    {
        for (int i = 0; i < this.Count; i++)
        {
            if (string.Equals(parameterName, parameters[i].ParameterName))
            {
                return i;
            }
        }
        return -1;
    }

    protected override DbParameter GetParameter(int index)
        => parameters[index];

    protected override DbParameter? GetParameter(string parameterName)
    {
        foreach (var parameter in parameters)
        {
            if (string.Equals(parameter.ParameterName, parameterName))
            {
                return parameter;
            }
        }

        return default;
    }

    public override void Insert(int index, object value)
        => parameters.Insert(index, (DbParameter)value);

    public override void RemoveAt(int index)
        => parameters.RemoveAt(index);

    public override void RemoveAt(string parameterName)
        => parameters.RemoveAll(p => string.Equals(p.ParameterName, parameterName));

    public override void Remove(object value)
        => parameters.Remove((DbParameter)value);

    protected override void SetParameter(int index, DbParameter value)
        => parameters[index] = value;

    protected override void SetParameter(string parameterName, DbParameter value)
    {
        var index = IndexOf(parameterName);
        if (index > -1)
        {
            parameters[index] = value;
        }
    }

    IEnumerator<DbParameter> IEnumerable<DbParameter>.GetEnumerator() => parameters.GetEnumerator();
}

