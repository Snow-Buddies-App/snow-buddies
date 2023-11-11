using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Interfaces.IRepositories
{
    public interface IUserRepository: IGenericRepository<User>
    {
        IEnumerable<User> GetAllUsersWithProfile();
    }
}
