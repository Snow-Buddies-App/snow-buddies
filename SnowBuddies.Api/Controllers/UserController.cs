using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult<List<UserModel>> GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<UserModel> GetUserById(Guid userId)
        {
            return Ok(_userService.GetUserById(userId));
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserModel), 201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<UserModel> CreateUser(UserModel user)
        {
            var mappedUser = _mapper.Map<User>(user);
            return CreatedAtAction(nameof(GetUserById), new { userId = user.UserId }, _userService.CreateUser(mappedUser));
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<UserModel> UpdateUser(UserModel user)
        {
            var mappedUser = _mapper.Map<User>(user);
            return Ok(_userService.UpdateUser(mappedUser));
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(201)] 
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<UserModel> DeleteUser(UserModel user)
        {
        var mappedUser = _mapper.Map<User>(user);
          return Ok(_userService.DeleteUser(mappedUser));
        }

    }
}