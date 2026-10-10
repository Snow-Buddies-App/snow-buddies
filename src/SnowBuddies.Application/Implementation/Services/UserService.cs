using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var existingUser = await _userRepository.GetByIdAsync(userId);
            if (existingUser != null)
            {
                _userRepository.Remove(existingUser);
                await _userRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<UserDto?> UpdateUserAsync(UserDto user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.UserId);

            if (existingUser == null)
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
                throw new ArgumentNullException("Email and DisplayName are required fields");
            }
            await CheckIfUserExist(user.Email, user.DisplayName);
            var userProfile = new UserProfile();
            user.UserProfile = userProfile;
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }

        private async Task CheckIfUserExist(string email, string displayName)
        {

            if(await _userRepository.AnyAsync(u => u.Email == email || u.DisplayName == displayName)) 
            {
                throw new ArgumentException("User already exist");
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            Expression<Func<User, bool>> userEmail = u => u.Email == email;
            
            return await _userRepository.FindAsync(userEmail);
        }
    }
}
