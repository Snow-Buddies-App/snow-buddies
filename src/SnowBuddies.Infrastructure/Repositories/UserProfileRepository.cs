using SnowBuddies.Application.Interfaces.IRepositories;
using SnowBuddies.Domain.Entities;
using SnowBuddies.Infrastructure.Data;

namespace SnowBuddies.Infrastructure.Repositories
{
    public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(SnowBuddiesDbContext context) : base(context)
        {
        }
    }
}
