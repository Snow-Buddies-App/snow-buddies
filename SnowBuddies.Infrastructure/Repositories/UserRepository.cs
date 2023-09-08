using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using SnowBuddies.Application.Interfaces.IRepositories;
using SnowBuddies.Domain.Entities;
using SnowBuddies.Infrastructure.Data;

namespace SnowBuddies.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SnowBuddiesDbContext _context;
        public UserRepository(SnowBuddiesDbContext context)
        {
            _context = context;
        }
        public User CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User DeleteUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
            return user;
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(Guid userId)
        {
            return _context.Users.Find(userId);
        }

        public User UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }  
        

    }
}