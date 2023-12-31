using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SnowBuddies.Application.Dtos;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Implementation.Services
{
    public class ExternalLoginService: IExternalLoginService
    {
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public ExternalLoginService(IGoogleAuthService googleAuthService, IUserService userService, IConfiguration configuration)
        {
            _googleAuthService = googleAuthService;
            _userService = userService;
            _configuration = configuration;
        }

        public async Task LoginWithGoogleAsync(string accessToken)
        {
            await ValidateAccessToken(accessToken);

            var userInfo = await _googleAuthService.GetUserInfoAsync(accessToken);

            if (userInfo == null || string.IsNullOrWhiteSpace(userInfo.Email))
            {
                throw new ArgumentNullException();
            }

            var user = await _userService.GetUserByEmailAsync(userInfo.Email);

            if (user == null)
            {
                var newUser = new User()
                {
                    Email = userInfo.Email,
                    UserId = Guid.NewGuid(),
                    UserProfile = new UserProfile()
                    {
                        FirstName = userInfo.GivenName,
                        LastName = userInfo.FamilyName,
                    },
                    DisplayName = $"{userInfo.GivenName}_{userInfo.Email}"
                };

                await _userService.CreateUserAsync(newUser);
            }
        }

        private async Task ValidateAccessToken (string accessToken) 
        {
            var validatedTokenResult = await _googleAuthService.ValidateAccessTokenAsync(accessToken);

            if (validatedTokenResult.IssuedTo != _configuration.GetSection("Authentication:Google:ClientId").Value!)
            {
                throw new Exception("Invalid user");
            }
        }

    }
}
