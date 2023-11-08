using Microsoft.EntityFrameworkCore;
using Moq;
using SnowBuddies.Domain.Entities;
using SnowBuddies.Infrastructure.Data;
using SnowBuddies.Infrastructure.Repositories;

namespace UnitTests.Repositories
{
    public class UserRepositoryTest
    {

        [Fact]
        public void GetUserById_ShouldReturnUser_WhenUserExists()
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
                    DisplayName = "SpiderMonkey",
                    Email = "kenny98@gmail.com"
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => users.GetEnumerator());
            var mockDbContext = new Mock<SnowBuddiesDbContext>();
            mockDbContext.Setup(context => context.Users).Returns(mockDbSet.Object);

            var userRepository = new UserRepository(mockDbContext.Object);

            var user = userRepository.GetUserById(userId);

            Assert.NotNull(user);
        }

        [Fact]
        public void GetAllUsers_ShouldReturnAllUsers()
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
                    DisplayName = "SpiderMonkey",
                    Email = "kenny98@gmail.com"
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => users.GetEnumerator());
            var mockDbContext = new Mock<SnowBuddiesDbContext>();
            mockDbContext.Setup(context => context.Users).Returns(mockDbSet.Object);

            var userRepository = new UserRepository(mockDbContext.Object);

            var actualUsers = userRepository.GetAllUsers();
            Assert.Equal(users.ToList(), actualUsers);
        }

        [Fact]
        public void CreateUser_AddUserToDatabase()
        {
            var users = new List<User>();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.AsQueryable().Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.AsQueryable().Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => users.AsQueryable().GetEnumerator());

            var mockDbContext = new Mock<SnowBuddiesDbContext>();
            mockDbContext.Setup(context => context.Users).Returns(mockDbSet.Object);
            var userRepository = new UserRepository(mockDbContext.Object);
            var createdUser = new User()
            {
                UserId = Guid.NewGuid(),
                DisplayName = "Sigma",
                Email = "kenny98@gmail.com"
            };

            userRepository.CreateUser(createdUser);

            mockDbContext.Verify(context => context.Users.Add(It.Is<User>(user => user.UserId == createdUser.UserId)), Times.Once);
            mockDbContext.Verify(context => context.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteUser_ShouldRemoveUserFromDatabase()
        {
            var users = new List<User>()
            {
                new User
                {
                    UserId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa5"),
                    DisplayName = "SpiderMonkey",
                    Email = "johndwayne@gmail.com"
                },

                new User
                {
                    UserId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    DisplayName ="Sigma",
                    Email = "kenny98@gmail.com"
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => users.GetEnumerator());

            var mockDbContext = new Mock<SnowBuddiesDbContext>();
            mockDbContext.Setup(context => context.Users).Returns(mockDbSet.Object);
            var userRepository = new UserRepository(mockDbContext.Object);
            var userToDelete = users.FirstOrDefault(user => user.UserId == Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa5"));

            userRepository.DeleteUser(userToDelete);

            mockDbContext.Verify(context => context.Users.Remove(It.Is<User>(user => user.UserId == userToDelete.UserId)), Times.Once);
            mockDbContext.Verify(context => context.SaveChanges(), Times.Once);
        }

        [Fact]
        public void UpdateUser_ShouldUpdateUserInDatabase()
        {
            var userId = Guid.NewGuid();
            var users = new List<User>()
            {
                new User
                {
                    UserId = userId,
                    DisplayName = "SpiderMonkey",
                    Email = "johndwayne@gmail.com"
                },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => users.GetEnumerator());

            var mockDbContext = new Mock<SnowBuddiesDbContext>();
            mockDbContext.Setup(context => context.Users).Returns(mockDbSet.Object);
            var userRepository = new UserRepository(mockDbContext.Object);
            var updatedUser = users.First();

            userRepository.UpdateUser(updatedUser);

            mockDbContext.Verify(context => context.Users.Update(It.Is<User>(user => user.UserId == updatedUser.UserId)), Times.Once);
            mockDbContext.Verify(context => context.SaveChanges(), Times.Once);
        }
    }
}
