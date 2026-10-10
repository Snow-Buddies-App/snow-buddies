using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace SnowBuddies.Application.Implementation.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        public AuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user)
        {
            var key = GenerateSecurityKey();
            var credentials = GenerateSigningCredentials(key);

            var token = new JwtSecurityToken(
                    claims: GetClaims(user),
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private IEnumerable<Claim> GetClaims(User user) 
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
            }; 

            return claims;
        }

        private SymmetricSecurityKey GenerateSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token")?.Value!));
        }

        private SigningCredentials GenerateSigningCredentials(SymmetricSecurityKey key)
        {
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        }

    }
}
