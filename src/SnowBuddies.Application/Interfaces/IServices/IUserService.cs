using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnowBuddies.Application.Dtos;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsers();
        User? GetUserById(Guid userId);
        bool DeleteUser(Guid userId);
        User UpdateUser(User user);
        User CreateUser(User user);
    }
}
