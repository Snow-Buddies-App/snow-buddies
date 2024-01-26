using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SnowBuddies.Api.Models;
using SnowBuddies.Application.Dtos;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, IMapper mapper, IPasswordService passwordService, ILogger<UserController> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _passwordService = passwordService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (users == null || !users.Any())
            {
                Log.Information("No users found.");
                return NotFound();
            }
            Log.Information("Returning {UserCount} users.", users?.Count() ?? 0);

            return Ok(users);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var existingUser = await _userService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                Log.Information("No user found.");
                return NotFound("User doesn't exist");
            }
            Log.Information("Returning user by ID: {UserId}", existingUser.UserId);

            return Ok(existingUser);
        }

        [HttpPost("Register")]
        [ProducesResponseType(typeof(UserModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateUser(UserModel userModel)
        {
            if (userModel == null)
            {
                Log.Warning("Invalid user data provided.");
                return BadRequest("Invalid user data.");
            }
            _passwordService.CreatePasswordHash(userModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User()
            {
                DisplayName = userModel.DisplayName,
                Email = userModel.Email,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash,
            };

            Log.Information("User created successfully. User ID: {Email}", newUser.Email);
            await _userService.CreateUserAsync(newUser);

            var responseUserModel = _mapper.Map<UserDto>(newUser);

            return CreatedAtAction(nameof(GetUserById), new { userId = newUser.UserId }, responseUserModel);
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UserModel userModel)
        {
            var existingUser = await _userService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                Log.Information("User not found to be updated.");
                return NotFound("User not found");
            }
            _mapper.Map(userModel, existingUser);

            var updatedUser = await _userService.UpdateUserAsync(existingUser);
            if (updatedUser == null)
            {
                Log.Information("User not updated.");
                return NotFound();
            }

            return Ok(updatedUser);
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var isDeleted = await _userService.DeleteUserAsync(userId);
            if (!isDeleted)
            {
                Log.Information("User not found to be deleted.");
                return NotFound("User not found");
            }
            Log.Information("User deleted successfully. User ID: {UserId}", userId);

            return NoContent();
        }
    }
}
