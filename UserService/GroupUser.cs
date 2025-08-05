namespace UserService;

internal sealed class GroupUser
{
    public required int GroupId { get; init; }
    public required int UserId { get; init; }
}