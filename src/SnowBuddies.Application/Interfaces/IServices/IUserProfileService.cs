using SnowBuddies.Application.Dtos;

namespace SnowBuddies.Application.Interfaces.IServices
{
    public interface IUserProfileService
    {
        Task<IEnumerable<UserProfileDto>> GetAllUserProfilesAsync();
        Task<UserProfileDto?> GetUserProfileByIdAsync(Guid userProfileId);
        Task<UserProfileDto?> GetUserProfileByUserIdAsync(Guid userId);
        Task<UserProfileDto> UpdateUserProfileAsync(UserProfileDto userProfile);
    }
}
