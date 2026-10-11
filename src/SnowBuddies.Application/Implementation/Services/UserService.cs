using System.Linq.Expressions;
using SnowBuddies.Application.Dtos;
using SnowBuddies.Application.Interfaces.IRepositories;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Application.Mapping;
using SnowBuddies.Domain.Entities;
using SnowBuddies.Domain.Exceptions;

namespace SnowBuddies.Application.Implementation.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => u.ToDto()).ToList();
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user?.ToDto();
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

        public async Task<UserDto> UpdateUserAsync(UserDto user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.UserId)
                ?? throw new NotFoundException("User not found");

            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.DisplayName))
            {
                throw new BadRequestException("Email and DisplayName are required fields");
            }

            var email = NormalizeEmail(user.Email);
            if (await _userRepository.AnyAsync(u => u.UserId != user.UserId && (u.Email == email || u.DisplayName == user.DisplayName)))
            {
                throw new ConflictException("Email or display name is already taken");
            }

            existingUser.Email = email;
            existingUser.DisplayName = user.DisplayName;
            _userRepository.Update(existingUser);
            await _userRepository.SaveChangesAsync();

            return existingUser.ToDto();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user?.Email) || string.IsNullOrWhiteSpace(user?.DisplayName))
            {
                throw new BadRequestException("Email and DisplayName are required fields");
            }
            user.Email = NormalizeEmail(user.Email);
            await CheckIfUserExist(user.Email, user.DisplayName);

            // Keep a profile supplied by the caller (e.g. names from Google); otherwise start with an empty one.
            user.UserProfile ??= new UserProfile();
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return user;
        }

        private async Task CheckIfUserExist(string email, string displayName)
        {
            if (await _userRepository.AnyAsync(u => u.Email == email || u.DisplayName == displayName))
            {
                throw new ConflictException("User already exists");
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var normalizedEmail = NormalizeEmail(email);
            Expression<Func<User, bool>> userEmail = u => u.Email == normalizedEmail;

            return await _userRepository.FindAsync(userEmail);
        }

        // Emails are stored trimmed and lower-cased so lookups are case-insensitive.
        private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

        public Task<bool> DisplayNameExistsAsync(string displayName)
        {
            return _userRepository.AnyAsync(u => u.DisplayName == displayName);
        }
    }
}
