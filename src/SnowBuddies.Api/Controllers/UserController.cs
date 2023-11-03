using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SnowBuddies.Api.Models;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            if (users == null)
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
        public IActionResult GetUserById(Guid userId)
        {
            var existingUser = _userService.GetUserById(userId);
            if (existingUser == null)
            {
                return NotFound("User doesn't exist");
            }
            return Ok(existingUser);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateUser(UserModel user)
        {
            var mappedUser = _mapper.Map<User>(user);
            _userService.CreateUser(mappedUser);

            return CreatedAtAction(nameof(GetUserById), new { userId = mappedUser.UserId }, mappedUser);
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult UpdateUser(Guid userId, [FromBody] UserModel userModel)
        {
            if (userId != userModel.UserId)
            {
                return BadRequest();
            }
            var existingUser = _mapper.Map<User>(userModel);
            var updatedUser = _userService.UpdateUser(existingUser);
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
        public async Task<IActionResult> DeleteUser(Guid userId)]
        {
            var isDeleted = _userService.DeleteUser(userId);
            if (!isDeleted)
            {
                return NotFound("User not found");
            }
            return NoContent();
        }
    }
}
