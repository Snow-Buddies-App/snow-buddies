using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SnowBuddies.Application.Dtos;
using SnowBuddies.Application.Implementation.Services;
using SnowBuddies.Application.Interfaces.IRepositories;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Domain.Entities;
using SnowBuddies.Domain.Exceptions;

namespace UnitTests.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private IUserService _userService;

        public UserServiceTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnMappedUserDtos()
        {
            var userId = Guid.NewGuid();
            var users = new List<User>()
            {
                new User
                {
                    UserId = userId,
                    DisplayName = "SpiderMonk",
                    Email = "johndwayne@gmail.com"
                },
                new User
                {
                    UserId = userId,
                    DisplayName = "Kenny98",
                    Email = "kenny98@gmail.com"
                }
            };

            _mockUserRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);

            var result = await _userService.GetAllUsersAsync();
            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count());
            Assert.IsAssignableFrom<IEnumerable<UserDto>>(result);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnEmptyCollection_WhenRepositoryReturnsEmpty()
        {
            var users = new List<User>();
            _mockUserRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);

            var result = await _userService.GetAllUsersAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser()
        {
            var userId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa5");
            var user = new User()
            {
                UserId = userId,
                DisplayName = "Mystery",
                Email = "mysteryman@gmail.com"
            };
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(user);

            var expectedUser = new User()
            {
                UserId = userId,
                DisplayName = "Mystery",
                Email = "mysteryman@gmail.com"
            };

            var result = await _userService.GetUserByIdAsync(userId);

            Assert.NotNull(result);
            Assert.IsType<UserDto>(result);
            Assert.Equal(expectedUser.DisplayName, result.DisplayName);
            Assert.Equal(expectedUser.UserId, result.UserId);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldAddUserToDatabase()
        {
            var userId = Guid.NewGuid();
            var user = new User()
            {
                UserId = userId,
                DisplayName = "Mystery",
                Email = "mysteryman@gmail.com"
            };

            _mockUserRepository.Setup(repo => repo.AddAsync(user, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var result = await _userService.CreateUserAsync(user);

            Assert.NotNull(result);
            Assert.IsType<User>(result);
            _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnFalseIfUserAlreadyExists()
        {
            var userId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var existingUser = new User()
            {
                UserId = userId,
                DisplayName = "Vitya",
                Email = "vity@gmail.com"
            };

            _mockUserRepository.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(true);

            await Assert.ThrowsAsync<ConflictException>(() => _userService.CreateUserAsync(existingUser));
        }

        [Fact]
        public async Task UpdateUserAsync_ExistingUser_ShouldUpdateAndReturnUpdatedUser()
        {
            var userId = Guid.NewGuid();
            var exisitingUser = new User()
            {
                UserId = userId,
                DisplayName = "Mike007",
                Email = "Mike@gmail.com"
            };

            var updatedUser = new UserDto()
            {
                UserId = userId,
                DisplayName = "Blue",
                Email = "blueberry@gmail.com"
            };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(exisitingUser);
           

            var result = await _userService.UpdateUserAsync(updatedUser);

            Assert.NotNull(result);
            Assert.Equal(updatedUser.UserId, result.UserId);
            Assert.Equal(updatedUser.Email, result.Email);
            _mockUserRepository.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_NonExistingUser_ShouldThrowException() 
        {
            var userId = Guid.NewGuid();
            var nonExistingUser = new UserDto
            {
                UserId = userId,
                DisplayName = "NewDisplayName",
                Email = "newemail@example.com"
            };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null!);

            await Assert.ThrowsAsync<NotFoundException>(() => _userService.UpdateUserAsync(nonExistingUser));
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteExistingUserFromDatabase() 
        {
            var userId = Guid.NewGuid();
            var existingUser = new User()
            {
                UserId = userId,
                DisplayName = "Mike007",
                Email = "Mike@gmail.com"
            };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            var result = await _userService.DeleteUserAsync(userId);

            Assert.True(result);

            _mockUserRepository.Verify(repo => repo.Remove(It.IsAny<User>()), Times.Once);
            _mockUserRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFalse()
        {
            var userId = Guid.NewGuid();

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User)null!);
            var result = await _userService.DeleteUserAsync(userId);

            Assert.False(result);
            
            _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(repo => repo.Remove(It.IsAny<User>()), Times.Never);
            _mockUserRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldKeepSuppliedProfile()
        {
            var profile = new UserProfile { FirstName = "Jane", LastName = "Doe" };
            var user = new User
            {
                DisplayName = "Jane_123456",
                Email = "jane@example.com",
                UserProfile = profile
            };

            var result = await _userService.CreateUserAsync(user);

            Assert.Same(profile, result.UserProfile);
            Assert.Equal("Jane", result.UserProfile!.FirstName);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateEmptyProfileWhenNoneSupplied()
        {
            var user = new User { DisplayName = "Kenny98", Email = "kenny98@gmail.com" };

            var result = await _userService.CreateUserAsync(user);

            Assert.NotNull(result.UserProfile);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldNormalizeEmail()
        {
            var user = new User { DisplayName = "Kenny98", Email = "  Kenny98@Gmail.COM " };

            var result = await _userService.CreateUserAsync(user);

            Assert.Equal("kenny98@gmail.com", result.Email);
        }

        [Fact]
        public async Task UpdateUserAsync_TakenEmailOrDisplayName_ShouldThrowConflict()
        {
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(new User { UserId = userId, DisplayName = "Mike007", Email = "mike@gmail.com" });
            _mockUserRepository.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(true);

            await Assert.ThrowsAsync<ConflictException>(() => _userService.UpdateUserAsync(new UserDto
            {
                UserId = userId,
                DisplayName = "Taken",
                Email = "taken@gmail.com"
            }));
            _mockUserRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }
    }
}
