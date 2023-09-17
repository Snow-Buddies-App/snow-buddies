using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Interfaces.IServices
{
    public interface IUserService
    {
        public List<User> GetAllUsers();
        public User GetUserById(Guid userId);
        public User DeleteUser(User user);
        public User UpdateUser(User user);
        public User CreateUser(User user);
    }
}