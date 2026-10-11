using System.Security.Cryptography;
using System.Text;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Domain.Entities;
using SnowBuddies.Domain.Exceptions;

namespace SnowBuddies.Application.Implementation.Services
{
    public class ExternalLoginService : IExternalLoginService
    {
        private const int DisplayNameMaxLength = 30;
        private const int NameMaxLength = 30;
        private const int DisplayNameAttempts = 10;

        private readonly IGoogleAuthService _googleAuthService;
        private readonly IUserService _userService;

        public ExternalLoginService(IGoogleAuthService googleAuthService, IUserService userService)
        {
            _googleAuthService = googleAuthService;
            _userService = userService;
        }

        public async Task<User> LoginWithGoogleAsync(string idToken)
        {
            var userInfo = await _googleAuthService.ValidateIdTokenAsync(idToken);

            if (string.IsNullOrWhiteSpace(userInfo.Email) || !userInfo.EmailVerified)
            {
                throw new UnauthorizedException("Google account email is not verified");
            }

            var existingUser = await _userService.GetUserByEmailAsync(userInfo.Email);
            if (existingUser != null)
            {
                return existingUser;
            }

            var newUser = new User()
            {
                Email = userInfo.Email,
                DisplayName = await GenerateUniqueDisplayNameAsync(userInfo.GivenName, userInfo.Email),
                UserProfile = new UserProfile()
                {
                    FirstName = Truncate(userInfo.GivenName, NameMaxLength),
                    LastName = Truncate(userInfo.FamilyName, NameMaxLength),
                    ProfilePicture = userInfo.Picture
                }
            };

            return await _userService.CreateUserAsync(newUser);
        }

        private async Task<string> GenerateUniqueDisplayNameAsync(string? givenName, string email)
        {
            var baseName = Sanitize(givenName);
            if (baseName.Length == 0)
            {
                baseName = Sanitize(email.Split('@')[0]);
            }
            if (baseName.Length == 0)
            {
                baseName = "buddy";
            }

            // Leave room for "_" plus a 6 digit suffix.
            baseName = Truncate(baseName, DisplayNameMaxLength - 7)!;

            for (var attempt = 0; attempt < DisplayNameAttempts; attempt++)
            {
                var candidate = $"{baseName}_{RandomNumberGenerator.GetInt32(0, 1_000_000):D6}";
                if (!await _userService.DisplayNameExistsAsync(candidate))
                {
                    return candidate;
                }
            }

            throw new ConflictException("Could not generate a unique display name");
        }

        private static string Sanitize(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            foreach (var c in value)
            {
                if (char.IsLetterOrDigit(c) || c == '_' || c == '.')
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }

        private static string? Truncate(string? value, int maxLength)
        {
            return value != null && value.Length > maxLength ? value[..maxLength] : value;
        }
    }
}
