using SnowBuddies.Application.Interfaces.IRepositories;
using SnowBuddies.Domain.Entities;
using SnowBuddies.Infrastructure.Data;

namespace SnowBuddies.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly SnowBuddiesDbContext _context;
        public UserRepository(SnowBuddiesDbContext context) : base(context)
        {
            _context = context;
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAllUsers()
        {
            var users = _context.Users.ToList();
            return users;
        }

        public User? GetUserById(Guid userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserId == userId);
            return user;
        }

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
