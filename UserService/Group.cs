namespace UserService;

internal sealed class Group
{
    public int Id { get; }
    public required string Name { get; set; }
    public List<GroupUser> Users { get; } = [];

    public void AddUser(int userId)
    {
        if(Users.Any(x => x.UserId == userId))
        {
            return;
        }

        Users.Add(new GroupUser
        {
            UserId = userId,
            GroupId = Id
        });
    }

    public void RemoveUser(int userId)
    {
        var userToRemove = Users.FirstOrDefault(x => x.UserId == userId);

        if (userToRemove != null)
        {
            Users.Remove(userToRemove);
        }
    }
}