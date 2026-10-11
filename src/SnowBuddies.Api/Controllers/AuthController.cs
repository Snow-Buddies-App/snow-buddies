using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SnowBuddies.Api.Models;
using SnowBuddies.Application.Dtos;
using SnowBuddies.Application.Interfaces.IServices;

namespace SnowBuddies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting(RateLimitPolicies.Auth)]
    public class AuthController : ControllerBase
    {
        private const string InvalidCredentialsMessage = "Invalid email or password";

        private readonly IPasswordService _passwordService;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IExternalLoginService _externalLoginService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IExternalLoginService externalLoginService, IPasswordService passwordService, IUserService userService, IAuthenticationService authenticationService, ILogger<AuthController> logger)
        {
            _passwordService = passwordService;
            _userService = userService;
            _authenticationService = authenticationService;
            _externalLoginService = externalLoginService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(AuthResult), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login(UserLoginModel loginUserRequest)
        {
            if (string.IsNullOrWhiteSpace(loginUserRequest.Email) || string.IsNullOrWhiteSpace(loginUserRequest.Password))
            {
                _logger.LogWarning("Invalid user login request. Email or password is empty.");
                return BadRequest("Invalid user");
            }

            var user = await _userService.GetUserByEmailAsync(loginUserRequest.Email);

            // Unknown email and wrong password get the same response so callers can't probe which emails are registered.
            if (user == null || !_passwordService.VerifyPasswordHash(loginUserRequest.Password, user.PasswordHash, user.PasswordSalt))
            {
                _logger.LogInformation("Failed login attempt. Email: {UserEmail}", loginUserRequest.Email);
                return Unauthorized(InvalidCredentialsMessage);
            }

            return Ok(_authenticationService.CreateToken(user));
        }

        [HttpPost("Google-sign-in")]
        [ProducesResponseType(typeof(AuthResult), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> LoginWithGoogle(GoogleSignInModel request)
        {
            if (string.IsNullOrWhiteSpace(request.IdToken))
            {
                _logger.LogWarning("Invalid Google sign-in request. ID token is empty.");
                return BadRequest("Google ID token is required");
            }

            var user = await _externalLoginService.LoginWithGoogleAsync(request.IdToken);
            return Ok(_authenticationService.CreateToken(user));
        }
    }
}
