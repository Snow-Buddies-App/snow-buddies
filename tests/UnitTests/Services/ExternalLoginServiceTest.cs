using System.Linq.Expressions;
using Moq;
using SnowBuddies.Application.Dtos;
using SnowBuddies.Application.Implementation.Services;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Domain.Entities;
using SnowBuddies.Domain.Exceptions;

namespace UnitTests.Services
{
    public class ExternalLoginServiceTest
    {
        private readonly Mock<IGoogleAuthService> _mockGoogleAuthService = new();
        private readonly Mock<IUserService> _mockUserService = new();
        private readonly ExternalLoginService _externalLoginService;

        public ExternalLoginServiceTest()
        {
            _mockUserService.Setup(s => s.CreateUserAsync(It.IsAny<User>())).ReturnsAsync((User u) => u);
            _externalLoginService = new ExternalLoginService(_mockGoogleAuthService.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task LoginWithGoogleAsync_ExistingUser_ShouldReturnUserWithoutCreating()
        {
            var existingUser = new User { UserId = Guid.NewGuid(), DisplayName = "Jane", Email = "jane@example.com" };
            SetupGoogleUser(new GoogleUserInfo { Email = "jane@example.com", EmailVerified = true });
            _mockUserService.Setup(s => s.GetUserByEmailAsync("jane@example.com")).ReturnsAsync(existingUser);

            var result = await _externalLoginService.LoginWithGoogleAsync("token");

            Assert.Same(existingUser, result);
            _mockUserService.Verify(s => s.CreateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task LoginWithGoogleAsync_NewUser_ShouldCreateUserWithProfileAndValidDisplayName()
        {
            SetupGoogleUser(new GoogleUserInfo
            {
                Email = "a.very.long.email.address.for.testing@example.com",
                EmailVerified = true,
                GivenName = "Bartholomew-Maximilian Alexander",
                FamilyName = "Montgomery-Worthington-Fitzgerald",
                Picture = "https://example.com/pic.png"
            });

            var result = await _externalLoginService.LoginWithGoogleAsync("token");

            Assert.NotNull(result.DisplayName);
            Assert.True(result.DisplayName!.Length <= 30);
            Assert.StartsWith("BartholomewMaximilianA", result.DisplayName);
            Assert.Null(result.PasswordHash);
            Assert.Equal("Bartholomew-Maximilian Alexand", result.UserProfile!.FirstName);
            Assert.True(result.UserProfile.LastName!.Length <= 30);
            Assert.Equal("https://example.com/pic.png", result.UserProfile.ProfilePicture);
            _mockUserService.Verify(s => s.CreateUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task LoginWithGoogleAsync_NoGivenName_ShouldUseEmailForDisplayName()
        {
            SetupGoogleUser(new GoogleUserInfo { Email = "snow.rider@example.com", EmailVerified = true });

            var result = await _externalLoginService.LoginWithGoogleAsync("token");

            Assert.StartsWith("snow.rider_", result.DisplayName);
        }

        [Fact]
        public async Task LoginWithGoogleAsync_UnverifiedEmail_ShouldThrowUnauthorized()
        {
            SetupGoogleUser(new GoogleUserInfo { Email = "jane@example.com", EmailVerified = false });

            await Assert.ThrowsAsync<UnauthorizedException>(() => _externalLoginService.LoginWithGoogleAsync("token"));
            _mockUserService.Verify(s => s.CreateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task LoginWithGoogleAsync_DisplayNameAlwaysTaken_ShouldThrowConflict()
        {
            SetupGoogleUser(new GoogleUserInfo { Email = "jane@example.com", EmailVerified = true, GivenName = "Jane" });
            _mockUserService.Setup(s => s.DisplayNameExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            await Assert.ThrowsAsync<ConflictException>(() => _externalLoginService.LoginWithGoogleAsync("token"));
        }

        private void SetupGoogleUser(GoogleUserInfo userInfo)
        {
            _mockGoogleAuthService.Setup(s => s.ValidateIdTokenAsync("token")).ReturnsAsync(userInfo);
        }
    }
}
