using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SnowBuddies.Api.Extensions;
using SnowBuddies.Api.Models;
using SnowBuddies.Application.Dtos;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Application.Mapping;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IPasswordService passwordService, ILogger<UserController> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _passwordService = passwordService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            _logger.LogInformation("Returning {UserCount} users.", users.Count());

            return Ok(users);
        }

        [HttpGet("me")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            return await GetUserById(userId.Value);
        }

        [HttpGet("{userId:guid}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var existingUser = await _userService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                _logger.LogInformation("No user found. User ID: {UserId}", userId);
                return NotFound("User doesn't exist");
            }

            return Ok(existingUser);
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        [EnableRateLimiting(RateLimitPolicies.Auth)]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(429)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateUser(UserModel userModel)
        {
            _passwordService.CreatePasswordHash(userModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User()
            {
                DisplayName = userModel.DisplayName,
                Email = userModel.Email,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash,
            };

            await _userService.CreateUserAsync(newUser);
            _logger.LogInformation("User created successfully. User ID: {UserId}", newUser.UserId);

            return CreatedAtAction(nameof(GetUserById), new { userId = newUser.UserId }, newUser.ToDto());
        }

        [HttpPut("{userId:guid}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserModel userModel)
        {
            if (User.GetUserId() != userId)
            {
                return Forbid();
            }

            var updatedUser = await _userService.UpdateUserAsync(new UserDto
            {
                UserId = userId,
                DisplayName = userModel.DisplayName,
                Email = userModel.Email
            });

            return Ok(updatedUser);
        }

        [HttpDelete("{userId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            if (User.GetUserId() != userId)
            {
                return Forbid();
            }

            var isDeleted = await _userService.DeleteUserAsync(userId);
            if (!isDeleted)
            {
                _logger.LogInformation("User not found to be deleted. User ID: {UserId}", userId);
                return NotFound("User not found");
            }
            _logger.LogInformation("User deleted successfully. User ID: {UserId}", userId);

            return NoContent();
        }
    }
}
