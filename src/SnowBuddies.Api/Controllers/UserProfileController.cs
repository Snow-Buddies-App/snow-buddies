using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnowBuddies.Api.Extensions;
using SnowBuddies.Api.Models;
using SnowBuddies.Application.Dtos;
using SnowBuddies.Application.Interfaces.IServices;

namespace SnowBuddies.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly ILogger<UserProfileController> _logger;

        public UserProfileController(IUserProfileService userProfileService, ILogger<UserProfileController> logger)
        {
            _userProfileService = userProfileService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserProfileDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUserProfilesAsync()
        {
            var userProfiles = await _userProfileService.GetAllUserProfilesAsync();
            return Ok(userProfiles);
        }

        [HttpGet("me")]
        [ProducesResponseType(typeof(UserProfileDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var userProfile = await _userProfileService.GetUserProfileByUserIdAsync(userId.Value);
            if (userProfile == null)
            {
                return NotFound("User Profile doesn't exist.");
            }
            return Ok(userProfile);
        }

        [HttpGet("{userProfileId:guid}")]
        [ProducesResponseType(typeof(UserProfileDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserProfileById(Guid userProfileId)
        {
            var existingUserProfile = await _userProfileService.GetUserProfileByIdAsync(userProfileId);
            if (existingUserProfile == null)
            {
                _logger.LogInformation("User profile not found. Profile ID: {UserProfileId}", userProfileId);
                return NotFound("User Profile doesn't exist.");
            }
            return Ok(existingUserProfile);
        }

        [HttpPut("edit-profile/{userProfileId:guid}")]
        [ProducesResponseType(typeof(UserProfileDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUserProfile(Guid userProfileId, [FromBody] UserProfileModel userProfileModel)
        {
            var existingUserProfile = await _userProfileService.GetUserProfileByIdAsync(userProfileId);
            if (existingUserProfile == null)
            {
                _logger.LogInformation("User profile not found to be updated. Profile ID: {UserProfileId}", userProfileId);
                return NotFound("User Profile not found");
            }

            if (existingUserProfile.UserId != User.GetUserId())
            {
                return Forbid();
            }

            existingUserProfile.ProfilePicture = userProfileModel.ProfilePicture;
            existingUserProfile.Bio = userProfileModel.Bio;
            existingUserProfile.FirstName = userProfileModel.FirstName;
            existingUserProfile.LastName = userProfileModel.LastName;
            existingUserProfile.DateOfBirth = userProfileModel.DateOfBirth;

            var updatedUserProfile = await _userProfileService.UpdateUserProfileAsync(existingUserProfile);

            return Ok(updatedUserProfile);
        }
    }
}
