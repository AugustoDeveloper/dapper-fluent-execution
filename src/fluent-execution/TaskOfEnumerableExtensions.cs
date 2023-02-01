namespace Dapper.FluentExecution;

///<summary>
///It's helper struct to allows returns a async result at fluent way
///</summary>
public static class TaskOfEnumerableExtensions
{
    ///<summary>
    ///Performs awaitable result as a <see cref="List{T}"/> of <typeparamref name="T"/>
    ///</summary>
    ///<returns>A list of <typeparamref name="T"/></returns>
    public static async Task<List<T>> ToListAsync<T>(this Task<IEnumerable<T>> taskResult)
    {
        var newResult = await taskResult;

        return newResult.ToList();
    }

    ///<summary>
    ///Performs an awaitable result as an <see cref="Array"/> of <typeparamref name="T"/>
    ///</summary>
    ///<returns>An array of <typeparamref name="T"/></returns>
    public static async Task<T[]> ToArrayAsync<T>(this Task<IEnumerable<T>> taskResult)
    {
        var newResult = await taskResult;

        return newResult.ToArray();
    }
}
