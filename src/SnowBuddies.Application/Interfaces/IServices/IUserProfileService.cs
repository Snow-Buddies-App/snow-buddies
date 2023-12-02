using SnowBuddies.Domain.Entities;
using SnowBuddies.Application.Dtos; 

namespace SnowBuddies.Application.Interfaces.IServices
{
    public interface IUserProfileService
    {
        public Task<IEnumerable<UserProfileDto>> GetAllUserProfilesAsync();
        public Task<UserProfileDto?> GetUserProfileByIdAsync(Guid userProfileId);
        Task<UserProfileDto?> UpdateUserProfileAsync(UserProfileDto userProfile);
    }
}
