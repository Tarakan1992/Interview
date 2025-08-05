using Microsoft.EntityFrameworkCore;

namespace UserService;

internal sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(e => e.Id);
        });

        modelBuilder.Entity<GroupUser>(builder =>
        {
            builder.HasKey(e => new { e.GroupId, e.UserId });
        });

        modelBuilder.Entity<Group>(builder =>
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(e => e.Users).WithOne().HasForeignKey(e => e.GroupId);
        });

        base.OnModelCreating(modelBuilder);
    }
}