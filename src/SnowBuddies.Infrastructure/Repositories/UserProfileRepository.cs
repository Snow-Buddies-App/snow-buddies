using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnowBuddies.Application.Interfaces.IRepositories;
using SnowBuddies.Domain.Entities;
using SnowBuddies.Infrastructure.Data;

namespace SnowBuddies.Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly SnowBuddiesDbContext _context;
        public UserProfileRepository(SnowBuddiesDbContext context)
        {
            _context = context;
        }
       public List<UserProfile> GetAllUserProfiles()
        {
            return _context.UserProfiles.ToList();
        }

        public UserProfile GetUserProfileById(Guid userProfileId)
        {
            return _context.UserProfiles.Find(userProfileId);

        }

        public void UpdateUserProfile(UserProfile userProfile)
        {
            _context.UserProfiles.Update(userProfile);
            _context.SaveChanges();
        }

        public void CreateUserProfile(UserProfile userProfile)
        {
            _context.UserProfiles.Add(userProfile);
            _context.SaveChanges();
        }

        public void DeleteUserProfile(UserProfile userProfile)
        {
            _context.UserProfiles.Remove(userProfile);
            _context.SaveChanges();
        }  
    }
}