using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using SnowBuddies.Domain.Entities;
using SnowBuddies.Domain.Entities.Enums;
using SnowBuddies.Infrastructure.Data;
using SnowBuddies.Infrastructure.Repositories;

namespace UnitTests.Repositories
{
    public class UserRepositoryTest
    {

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
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
            mockDbContext.Setup(context => context.Set<User>()).Returns(mockDbSet.Object);

            var userRepository = new GenericRepository<User>(mockDbContext.Object);

            var user = await userRepository.FindAsync(x => x.UserId == userId);

            
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
            mockDbContext.Setup(context => context.Set<User>()).Returns(mockDbSet.Object);

            var userRepository = new GenericRepository<User>(mockDbContext.Object);

            var actualUsers = userRepository.GetAllAsync();
           
        }

        [Fact]
        public async Task CreateUser_AddUserToDatabaseAsync()
        {
            var users = new List<User>();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.AsQueryable().Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.AsQueryable().Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => users.AsQueryable().GetEnumerator());

            var mockDbContext = new Mock<SnowBuddiesDbContext>();
            mockDbContext.Setup(context => context.Set<User>()).Returns(mockDbSet.Object);
            var userRepository = new GenericRepository<User>(mockDbContext.Object);
            var createdUser = new User()
            {
                UserId = Guid.NewGuid(),
                DisplayName = "SpiderMonkey",
                Email = "kenny98@gmail.com"  
            };

            await userRepository.AddAsync(createdUser);

            mockDbContext.Verify(context => context.Set<User>().Add(It.Is<User>(user => user.UserId == createdUser.UserId)), Times.Once);
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
                    DisplayName = "SpiderMonk",
                    Email = "kenny98@gmail.com"
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => users.GetEnumerator());

            var mockDbContext = new Mock<SnowBuddiesDbContext>();
            mockDbContext.Setup(context => context.Set<User>()).Returns(mockDbSet.Object);
            var userRepository = new GenericRepository<User>(mockDbContext.Object);
            var userToDelete = users.First(user => user.UserId == Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa5"));

            userRepository.Remove(userToDelete);

            mockDbContext.Verify(context => context.Set<User>().Remove(It.Is<User>(user => user.UserId == userToDelete.UserId)), Times.Once);
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
            mockDbContext.Setup(context => context.Set<User>()).Returns(mockDbSet.Object);
            var userRepository = new GenericRepository<User>(mockDbContext.Object);
            var updatedUser = users.First();
            updatedUser.DisplayName = "Fish";

            userRepository.Update(updatedUser);

            mockDbContext.Verify(context => context.Set<User>().Update(It.Is<User>(user => user.UserId == updatedUser.UserId)), Times.Once);
        }
    }
}
