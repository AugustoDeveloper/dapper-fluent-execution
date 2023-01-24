namespace Dapper.FluentExecution;

public struct AsyncResult<T>
{
    private readonly Task<IEnumerable<T>> taskResult;

    internal AsyncResult(Task<IEnumerable<T>> result)
    {
        this.taskResult = result;
    }

    public async Task<List<T>> ToListAsync()
    {
        var newResult = await taskResult;

        return newResult.ToList();
    }

    public async Task<T[]> ToArrayAsync()
    {
        var newResult = await taskResult;

        return newResult.ToArray();
    }
}
