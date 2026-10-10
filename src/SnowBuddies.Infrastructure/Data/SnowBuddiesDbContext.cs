using Microsoft.EntityFrameworkCore;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Infrastructure.Data;

public class SnowBuddiesDbContext: DbContext
{
    public SnowBuddiesDbContext(DbContextOptions<SnowBuddiesDbContext> options): base(options)
    {
    }

    public SnowBuddiesDbContext(){}
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.DisplayName).IsUnique();
    }
}
