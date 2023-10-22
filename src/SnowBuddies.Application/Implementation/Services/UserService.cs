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

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        private async Task<bool> CheckIfUserExist(string email, string displayName) 
        {
            var users = await _userRepository.GetAllAsync();

            if(!users.Any(u => u.Email == email) && !users.Any(u => u.DisplayName == displayName)) 
            {
                return true;
            }
            return false;
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var existingUser = await _userRepository.GetByIdAsync(userId);
            if(existingUser != null) 
            {
                _userRepository.Remove(existingUser);
                await _userRepository.SaveChangesAsync();
            }
            return false;
        }

        public async Task<UserDto?> UpdateUserAsync(UserDto user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.UserId);
            
            if(existingUser == null) 
            {
                throw new ArgumentNullException(nameof(user));
            }
            existingUser.Email = user.Email;
            existingUser.DisplayName = user.DisplayName;
            _userRepository.Update(existingUser);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserDto>(existingUser);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user?.Email) || string.IsNullOrWhiteSpace(user?.DisplayName))
            {
                throw new ArgumentException("Email and DisplayName are required fields");
            }
            await CheckIfUserExist(user.Email, user.DisplayName);

            var newUser = new User
            {
                UserId = user.UserId,
                Email = user.Email,
                DisplayName = user.DisplayName,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
                UserProfile = new UserProfile(),
            };
            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            return newUser;
        }

        public async Task<User?> CreateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user?.Email) || string.IsNullOrWhiteSpace(user?.DisplayName))
            {
                throw new ArgumentException("Email and DisplayName are required fields");
            }

            await CheckIfUserExist(user.Email, user.DisplayName);

            var newUser = new User
            {
                UserId = user.UserId,
                Email = user.Email,
                DisplayName = user.DisplayName,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
                UserProfile = new UserProfile(),
            };
            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            return newUser;
        }
    }
}
