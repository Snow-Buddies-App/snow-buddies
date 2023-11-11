using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Interfaces.IServices
{
    public interface IUserProfileService
    {
        public Task<IEnumerable<UserProfile>> GetAllAsync();
        public Task<UserProfile> CreateUserProfile(UserProfile userProfile);
    }
}
