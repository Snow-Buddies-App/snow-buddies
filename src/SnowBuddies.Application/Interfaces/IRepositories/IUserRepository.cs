using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        public IEnumerable<User> GetAllUsers();

        public User? GetUserById(Guid userId);

        public void DeleteUser(User user);

        public void UpdateUser(User user);

        public void CreateUser(User user);
    }
}
