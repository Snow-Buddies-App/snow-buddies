using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        public UserController(IUserService userService, IMapper mapper, IPasswordService passwordService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _passwordService = passwordService;

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
                return NotFound();
            }
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
                return NotFound("User doesn't exist");
            }
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

            await _userService.CreateUserAsync(newUser);
            var responseUserModel = new UserDto()
            {
                UserId = newUser.UserId,
                DisplayName = newUser.DisplayName,
                Email = newUser.Email,
            };
            return CreatedAtAction(nameof(GetUserById), new { userId = newUser.UserId }, responseUserModel);
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UserModel userModel)
        {
            var existingUser = await _userService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound("User not found");
            }
            _mapper.Map(userModel, existingUser);

            var updatedUser = await _userService.UpdateUserAsync(existingUser);
            if (updatedUser == null)
            {
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
                return NotFound("User not found");
            }
            return NoContent();
        }
    }
}
