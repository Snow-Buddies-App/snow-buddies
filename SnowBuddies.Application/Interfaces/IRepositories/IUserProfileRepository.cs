using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Interfaces.IRepositories
{
    public interface IUserProfileRepository
    {
        public List<UserProfile> GetAllUserProfiles();
        public UserProfile GetUserProfileById(Guid userProfileId);
        public UserProfile UpdateUserProfile(UserProfile userProfile);
        public UserProfile CreateUserProfile(UserProfile userProfile);
        public UserProfile DeleteUserProfile(UserProfile userProfile);
    }
}