using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Interfaces.IRepositories
{
    public interface IUserProfileRepository: IGenericRepository<UserProfile>
    {
    }
}
