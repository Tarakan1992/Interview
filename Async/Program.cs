var numberOfTasks = 1000;

var results = new List<bool>(numberOfTasks);

for(var i = 0; i < numberOfTasks; i++)
{
    results.Add(await SomeAsyncFunction());
}

var rate = results.Sum(x => x ? 1 : 0) * 100 / numberOfTasks;

Console.WriteLine(rate);


async Task<bool> SomeAsyncFunction()
{
    var beforeThreadId = Environment.CurrentManagedThreadId;

    await Task.CompletedTask;

    var afterThreadId = Environment.CurrentManagedThreadId;

    return beforeThreadId == afterThreadId;
}