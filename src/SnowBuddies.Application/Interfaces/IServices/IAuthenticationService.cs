using SnowBuddies.Application.Dtos;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Interfaces.IServices
{
    public interface IAuthenticationService
    {
        AuthResult CreateToken(User user);
    }
}
