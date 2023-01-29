namespace Dapper.FluentExecution;

///<summary>
///It's helper struct to allows returns a async result at fluent way
///</summary>
public struct AsyncResult<T>
{
    private readonly Task<IEnumerable<T>> taskResult;

    internal AsyncResult(Task<IEnumerable<T>> result)
    {
        this.taskResult = result;
    }

    ///<summary>
    ///Performs an awaitable result as <see cref="IEnumerable{T}"/> of <typeparamref name="T"/> 
    ///</summary>
    ///<returns>An IEnumerable of <typeparamref name="T"/></returns>
    public async Task<IEnumerable<T>> GetResultAsync() => await taskResult;

    ///<summary>
    ///Performs awaitable result as a <see cref="List{T}"/> of <typeparamref name="T"/>
    ///</summary>
    ///<returns>A list of <typeparamref name="T"/></returns>
    public async Task<List<T>> ToListAsync()
    {
        var newResult = await taskResult;

        return newResult.ToList();
    }

    ///<summary>
    ///Performs an awaitable result as an <see cref="Array"/> of <typeparamref name="T"/>
    ///</summary>
    ///<returns>An array of <typeparamref name="T"/></returns>
    public async Task<T[]> ToArrayAsync()
    {
        var newResult = await taskResult;

        return newResult.ToArray();
    }
}
