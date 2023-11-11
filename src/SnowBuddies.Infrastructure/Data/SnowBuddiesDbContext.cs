using SnowBuddies.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SnowBuddies.Infrastructure.Data;

public class SnowBuddiesDbContext: DbContext
{
    public SnowBuddiesDbContext(DbContextOptions<SnowBuddiesDbContext> options): base(options)
    {
    }

    public SnowBuddiesDbContext(){}
    public virtual DbSet<User> Users { get; set; }  
    public virtual DbSet<UserProfile> UserProfiles { get; set; }
}
