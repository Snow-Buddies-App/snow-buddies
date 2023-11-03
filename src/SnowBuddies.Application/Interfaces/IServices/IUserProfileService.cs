using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Interfaces.IServices
{
    public interface IUserProfileService
    {
        public List<UserProfile> GetAllUserProfiles();
        public UserProfile GetUserProfileById(Guid userProfileId);
        public void DeleteUserProfile(UserProfile userProfile);
        public void UpdateUserProfile(UserProfile userProfile);
        public void CreateUserProfile(UserProfile userProfile);
    }
}