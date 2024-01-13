using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Domain.Entities;
using SnowBuddies.Api.Models;
using SnowBuddies.Application.Dtos;


namespace SnowBuddies.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserProfileController> _logger;

        public UserProfileController(IUserProfileService userProfileService, IMapper mapper, ILogger<UserProfileController> logger)
        {
            _userProfileService = userProfileService;
            _mapper = mapper;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUserProfilesAsync()
        {
            var userProfiles = await _userProfileService.GetAllUserProfilesAsync();
            
            if (userProfiles == null || !userProfiles.Any())
            {
                return NotFound();
            }
            return Ok(userProfiles);
        }
        [HttpGet("{userProfileId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserProfileById(Guid userProfileId)
        {
            var existingUserProfile = await _userProfileService.GetUserProfileByIdAsync(userProfileId);
            if (existingUserProfile == null)
            {
                return NotFound("User Profile doesn't exist");
            }
            return Ok(existingUserProfile);
        }



        [HttpPut("edit-profile/{userProfileId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUserProfile(Guid userProfileId, [FromBody] UserProfileModel userProfileModel)
        {
            var existingUser = await _userProfileService.GetUserProfileByIdAsync(userProfileId);
            if (existingUser == null)
            {
                return NotFound("User Profile not found");
            }
            _mapper.Map(userProfileModel, existingUser);

           var updatedUserProfile = await _userProfileService.UpdateUserProfileAsync(existingUser);

            if (updatedUserProfile == null)
            {
                return NotFound();
            }

            return Ok(updatedUserProfile);
        }



    }

}
