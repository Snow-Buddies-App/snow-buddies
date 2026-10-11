using SnowBuddies.Application.Dtos;
using SnowBuddies.Application.Interfaces.IRepositories;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Application.Mapping;
using SnowBuddies.Domain.Exceptions;

namespace SnowBuddies.Application.Implementation.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<IEnumerable<UserProfileDto>> GetAllUserProfilesAsync()
        {
            var userProfiles = await _userProfileRepository.GetAllAsync();
            return userProfiles.Select(p => p.ToDto()).ToList();
        }

        public async Task<UserProfileDto?> GetUserProfileByIdAsync(Guid userProfileId)
        {
            var userProfile = await _userProfileRepository.GetByIdAsync(userProfileId);
            return userProfile?.ToDto();
        }

        public async Task<UserProfileDto?> GetUserProfileByUserIdAsync(Guid userId)
        {
            var userProfile = await _userProfileRepository.FindAsync(p => p.UserId == userId);
            return userProfile?.ToDto();
        }

        public async Task<UserProfileDto> UpdateUserProfileAsync(UserProfileDto userProfile)
        {
            var existingUserProfile = await _userProfileRepository.GetByIdAsync(userProfile.UserProfileId)
                ?? throw new NotFoundException("User Profile not found");

            existingUserProfile.FirstName = userProfile.FirstName;
            existingUserProfile.LastName = userProfile.LastName;
            existingUserProfile.Bio = userProfile.Bio;
            existingUserProfile.ProfilePicture = userProfile.ProfilePicture;
            existingUserProfile.DateOfBirth = userProfile.DateOfBirth;
            _userProfileRepository.Update(existingUserProfile);
            await _userProfileRepository.SaveChangesAsync();

            return existingUserProfile.ToDto();
        }
    }
}
