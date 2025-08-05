namespace UserService;

internal sealed class User
{
    public int Id { get; }
    public required string Username { get; init; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}