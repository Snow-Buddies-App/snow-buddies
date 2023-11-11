using SnowBuddies.Application.Interfaces.IRepositories;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Implementation.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<UserProfile> CreateUserProfile(UserProfile userProfile)
        {
            await _userProfileRepository.AddAsync(userProfile);
            await _userProfileRepository.SaveChangesAsync();
            return userProfile;
        }

        public async Task<IEnumerable<UserProfile>> GetAllAsync()
        {
            return await _userProfileRepository.GetAllAsync();
        }
    }
}
