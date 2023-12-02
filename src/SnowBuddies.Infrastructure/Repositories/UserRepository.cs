using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnowBuddies.Application.Interfaces.IRepositories;
using SnowBuddies.Domain.Entities;
using SnowBuddies.Infrastructure.Data;

namespace SnowBuddies.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(SnowBuddiesDbContext context) : base(context)
        { 
        }

        public IEnumerable<User> GetAllUsersWithProfile()
        {
            return _context.Users.Include(u => u.UserProfile).ToList();
        }
    }
}
