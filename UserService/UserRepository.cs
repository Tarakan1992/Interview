using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace UserService;

internal interface IUserRepository
{
    Task<IEnumerable<User>> GetUsers(IEnumerable<int> userIds);
    Task<IEnumerable<Group>> GetGroups(IEnumerable<int> groupIds);
    Task<IEnumerable<User>> GetAllUsers();
    Task<IEnumerable<User>> GetUsersByGroup(IEnumerable<int> groupIds);
}

internal sealed class UserRepository(IDbContextFactory<AppDbContext> dbContextFactory, IMemoryCache memoryCache) : IUserRepository
{
    public Task<IEnumerable<User>> GetAllUsers()
    {
        return GetAllUsersInternal();
    }

    public async Task<IEnumerable<User>> GetUsers(IEnumerable<int> userIds)
    {
        var users = await GetAllUsersInternal();

        return users.Where(x => userIds.Contains(x.Id)).ToList();
    }

    public async Task<IEnumerable<Group>> GetGroups(IEnumerable<int> groupIds)
    {
        using var dbContext = dbContextFactory.CreateDbContext();

        return await dbContext
            .Set<Group>()
            .Include(x => x.Users)
            .Where(x => groupIds.Contains(x.Id))
            .ToListAsync();
    }

    private async Task<IEnumerable<User>> GetAllUsersInternal()
    {
        using var dbContext = dbContextFactory.CreateDbContext();

        return await memoryCache.GetOrCreateAsync("users", async _ => await dbContext.Set<User>().ToListAsync())
            ?? [];
    }

    public async Task<IEnumerable<User>> GetUsersByGroup(IEnumerable<int> groupIds)
    {
        var groups = await GetGroups(groupIds);

        var userIds = groups.SelectMany(x => x.Users).Select(x => x.UserId);

        return await GetUsers(userIds);
    }
}