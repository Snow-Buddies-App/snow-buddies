using SnowBuddies.Application.Dtos;
using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Mapping
{
    public static class MappingExtensions
    {
        public static UserDto ToDto(this User user) => new()
        {
            UserId = user.UserId,
            DisplayName = user.DisplayName,
            Email = user.Email
        };

        public static UserProfileDto ToDto(this UserProfile profile) => new()
        {
            UserProfileId = profile.UserProfileId,
            UserId = profile.UserId,
            ProfilePicture = profile.ProfilePicture,
            Bio = profile.Bio,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            DateOfBirth = profile.DateOfBirth
        };
    }
}
