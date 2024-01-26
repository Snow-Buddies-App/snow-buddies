using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SnowBuddies.Application.Dtos;
using SnowBuddies.Application.Interfaces.IRepositories;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Implementation.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;
        public UserProfileService(IUserProfileRepository userProfileRepository, IMapper mapper)
        {
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserProfileDto>> GetAllUserProfilesAsync()
        {
            var userProfiles = await _userProfileRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserProfileDto>>(userProfiles);
        }

        public async Task<UserProfileDto?> GetUserProfileByIdAsync(Guid userId)
        {
            var user = await _userProfileRepository.GetByIdAsync(userId);
            return _mapper.Map<UserProfileDto>(user);
        }

        public async Task<UserProfileDto?> UpdateUserProfileAsync(UserProfileDto userProfile)
        {
            var existingUserProfile = await _userProfileRepository.GetByIdAsync(userProfile.UserProfileId);

            if (existingUserProfile == null)
            {
                throw new ArgumentNullException(nameof(userProfile));
            }

            existingUserProfile.FirstName = userProfile.FirstName;
            existingUserProfile.LastName = userProfile.LastName;
            existingUserProfile.Bio = userProfile.Bio;
            existingUserProfile.ProfilePicture = userProfile.ProfilePicture;
            existingUserProfile.DateOfBirth = userProfile.DateOfBirth;
            _userProfileRepository.Update(existingUserProfile);
            await _userProfileRepository.SaveChangesAsync();

            return _mapper.Map<UserProfileDto>(existingUserProfile);
        }
    }
}
