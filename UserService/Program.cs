using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService;

var serviceCollection = new ServiceCollection();

serviceCollection
    .AddDbContextFactory<AppDbContext>(x => x.UseInMemoryDatabase("users"))
    .AddMemoryCache()
    .AddSingleton<IUserRepository, UserRepository>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var dbContextFactory = serviceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();

await using var dbContext = await dbContextFactory.CreateDbContextAsync();

await dbContext.Set<User>().AddRangeAsync(
    [
        new User
        {
            Username = "Username_1",
            FirstName = "FirstName_1",
            LastName = "LastName_1"
        },
        new User
        {
            Username = "Username_2",
            FirstName = "FirstName_2",
            LastName = "LastName_2"
        },
        new User
        {
            Username = "Username_3",
            FirstName = "FirstName_3",
            LastName = "LastName_3"
        },
        new User
        {
            Username = "Username_4",
            FirstName = "FirstName_4",
            LastName = "LastName_4"
        },
    ]);

await dbContext.SaveChangesAsync();

var users = await dbContext.Set<User>().ToListAsync();

var group = new Group
{
    Name = "Main"
};

foreach(var user in users)
{
    group.AddUser(user.Id);
}

await dbContext.Set<Group>().AddAsync(group);

await dbContext.SaveChangesAsync();

var savedGroup = await dbContext.Set<Group>().Include(x => x.Users).FirstOrDefaultAsync();

if(savedGroup != null)
{
    Console.WriteLine($"Users in group: {string.Join(',', group.Users.Select(x => x.UserId))}");
}

var userRepository = serviceProvider.GetRequiredService<IUserRepository>();