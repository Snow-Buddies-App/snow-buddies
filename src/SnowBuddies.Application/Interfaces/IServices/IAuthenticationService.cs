using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnowBuddies.Application.Dtos;

namespace SnowBuddies.Application.Interfaces.IServices
{
    public interface IAuthenticationService
    {
        string CreateToken(UserDto user);

    }
}
