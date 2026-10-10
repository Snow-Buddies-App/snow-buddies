using SnowBuddies.Application.Dtos;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(Guid userId);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<UserDto?> UpdateUserAsync(UserDto user);
        Task<User> CreateUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
