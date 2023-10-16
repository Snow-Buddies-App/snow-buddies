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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public User? GetUserById(Guid userId)
        {
            return _userRepository.GetById(userId);
        }

        public bool DeleteUser(Guid userId)
        {
            var existingUser = _userRepository.GetById(userId);
            if (existingUser == null)
            {
                return false;
            }
            _userRepository.Remove(existingUser);
            return true;
        }

        public User UpdateUser(User user)
        {
            var existingUser = _userRepository.GetById(user.UserId);

            if (existingUser == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            
            existingUser.Email = user.Email;
            existingUser.DisplayName = user.DisplayName;
            _userRepository.Update(existingUser);

            return existingUser;
        }

        public User CreateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user?.Email) || string.IsNullOrWhiteSpace(user?.DisplayName))
            {
                throw new ArgumentException("Email and DisplayName are required fields");
            }

            CheckIfUserExist(user.Email, user.DisplayName);

            var newUser = new User
            {
                UserId = user.UserId,
                Email = user.Email,
                DisplayName = user.DisplayName,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
                UserProfile = new UserProfile(),
            };
    
            _userRepository.Add(newUser);
            _userRepository.SaveChanges();
            
            return newUser;
        }

        private bool CheckIfUserExist(string email, string displayName) 
        {
            var users = _userRepository.GetAll();

            var userWithEmail = users.Any(u => u.Email == email);

            var userWithDisplayName = users.Any(u => u.DisplayName == displayName);

            if (userWithEmail && userWithDisplayName)
            {
                return false;
            }

            return true;
        }
    }
}
