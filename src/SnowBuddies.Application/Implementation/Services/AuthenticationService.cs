using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using SnowBuddies.Application.Dtos;
using SnowBuddies.Application.Interfaces.IServices;

namespace SnowBuddies.Application.Implementation.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public string CreateToken(UserDto user)
        {
            throw new NotImplementedException();
        }

    }
}