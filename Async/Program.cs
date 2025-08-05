var numberOfTasks = 10000;

var tasks = new List<Task<bool>>(numberOfTasks);

for(var i = 0; i < numberOfTasks; i++)
{
    tasks.Add(SomeAsyncFunction());
}

await Task.WhenAll(tasks);

var rate = tasks.Select(x => x.Result).Sum(x => x ? 1 : 0) * 100 / numberOfTasks;

Console.WriteLine(rate);


async Task<bool> SomeAsyncFunction()
{
    var beforeThreadId = Environment.CurrentManagedThreadId;

    await Task.CompletedTask;

    var afterThreadId = Environment.CurrentManagedThreadId;

    return beforeThreadId == afterThreadId;
}