using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Interfaces.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public IEnumerable<User> GetAllUsers();

        public User? GetUserById(Guid userId);

        public void DeleteUser(User user);

        public void UpdateUser(User user);

        public void CreateUser(User user);
    }
}
