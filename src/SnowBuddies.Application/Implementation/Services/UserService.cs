using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnowBuddies.Application.Interfaces.IRepositories;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Implementation.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User GetUserById(Guid userId)
        {
            return _userRepository.GetUserById(userId);
        }

        public bool DeleteUser(Guid userId)
        {
            var existingUser = _userRepository.GetUserById(userId);
            if (existingUser == null) 
            {
                return false;
            }
            _userRepository.DeleteUser(existingUser);
            return true;
        }

        public User UpdateUser(User user)
        {
            var existingUser = _userRepository.GetUserById(user.UserId);
            if (existingUser == null) 
            {
                return null;
            }
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.DisplayName = user.DisplayName;
            existingUser.AccountStatus = user.AccountStatus;
            _userRepository.UpdateUser(existingUser);
            return existingUser;
        }

        public User CreateUser(User user)
        {
            _userRepository.CreateUser(user);
            return user;
        }
    }
}
