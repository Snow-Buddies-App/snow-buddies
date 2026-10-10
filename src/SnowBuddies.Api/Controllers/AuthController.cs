using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SnowBuddies.Api.Models;
using SnowBuddies.Application.Dtos;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IPasswordService _passwordService;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IExternalLoginService _externalLoginService;
        private readonly ILogger<AuthController> _logger;
        
        public AuthController(IExternalLoginService externalLoginService, IPasswordService passwordService, IUserService userService, IAuthenticationService authenticationService, ILogger<AuthController> logger)
        {
            _authenticationService = authenticationService;
            _passwordService = passwordService;
            _userService = userService;
            _authenticationService = authenticationService;
            _externalLoginService = externalLoginService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("Login")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Login(UserLoginModel loginUserRequest)
        {
            if (string.IsNullOrWhiteSpace(loginUserRequest.Email) || string.IsNullOrWhiteSpace(loginUserRequest.Password))
            {
                Log.Warning("Invalid user login request. Email or password is empty.");
                return BadRequest("Invalid user");
            }

            var user = await _userService.GetUserByEmailAsync(loginUserRequest.Email);

            if (user == null) 
            {
                Log.Information("User not found for login. Email: {UserEmail}", loginUserRequest.Email);
                return NotFound("User not found");
            }
             
            var isPasswordVerified = _passwordService.VerifyPasswordHash(loginUserRequest.Password, user.PasswordHash, user.PasswordSalt);

            if (!isPasswordVerified)
            {
                Log.Information("Incorrect password for user login. Email: {UserEmail}", loginUserRequest.Email);
                return Unauthorized("Incorrect password");
            }
            var token = _authenticationService.CreateToken(user);
            return Ok(token);
        }

        [HttpPost("Google-sign-in")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> LoginWithGoogle(string accessToken) 
        {
            if (string.IsNullOrWhiteSpace(accessToken)) 
            {
                Log.Warning("Invalid Google sign-in request. Access token is empty.");
                return BadRequest();
            }
            await _externalLoginService.LoginWithGoogleAsync(accessToken);
            return Ok();
        }
    }
}
