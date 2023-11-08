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

        public List<UserProfile> GetAllUserProfiles()
        {
            return _userProfileRepository.GetAllUserProfiles();
        }

        public UserProfile GetUserProfileById(Guid userProfileId)
        {
            return _userProfileRepository.GetUserProfileById(userProfileId);
        }
        public void DeleteUserProfile(UserProfile userProfile)
        {
            _userProfileRepository.DeleteUserProfile(userProfile);
        }

        public void UpdateUserProfile(UserProfile userProfile)
        {
            _userProfileRepository.UpdateUserProfile(userProfile);
        }
        public void CreateUserProfile(UserProfile userProfile)
        {
            _userProfileRepository.CreateUserProfile(userProfile);
        }

        public async Task<IEnumerable<UserProfile>> GetAllAsync()
        {
            return await _userProfileRepository.GetAllAsync();
        }


    }
}
