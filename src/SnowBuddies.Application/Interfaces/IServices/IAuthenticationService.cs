using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Interfaces.IServices
{
    public interface IAuthenticationService
    {
        string CreateToken(User user);

    }
}
