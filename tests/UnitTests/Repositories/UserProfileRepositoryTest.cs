using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using SnowBuddies.Domain.Entities.Enums;
using SnowBuddies.Domain.Entities;
using SnowBuddies.Infrastructure.Data;
using SnowBuddies.Infrastructure.Repositories;

namespace UnitTests.Repositories
{
    public class UserProfileRepositoryTest
    {
        [Fact]
        public void GetAll_ShouldReturnAllUserProfiles()
        {
            var userProfileId = Guid.NewGuid();
            var userProfiles = new List<UserProfile>()
            {
                new UserProfile{UserProfileId = userProfileId,
                        Bio = "I am a nerd",
                        ProfilePicture = "pic",
                        AccountStatus = AccountStatus.active,
                        FirstName = "Daniel",
                        LastName ="Kenneth",
                        DateOfBirth = DateTime.Now,
                        User = new User()
                       },

                new UserProfile{UserProfileId = userProfileId,
                        Bio = "I am a language nerd",
                        ProfilePicture = "pic",
                        FirstName = "Dan",
                        LastName ="Ken",
                        AccountStatus = AccountStatus.suspended,
                        User = new User()
                        }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<UserProfile>>();
            mockDbSet.As<IQueryable<UserProfile>>().Setup(m => m.Provider).Returns(userProfiles.Provider);
            mockDbSet.As<IQueryable<UserProfile>>().Setup(m => m.Expression).Returns(userProfiles.Expression);
            mockDbSet.As<IQueryable<UserProfile>>().Setup(m => m.ElementType).Returns(userProfiles.ElementType);
            mockDbSet.As<IQueryable<UserProfile>>().Setup(m => m.GetEnumerator()).Returns(() => userProfiles.GetEnumerator());
            var mockDbContext = new Mock<SnowBuddiesDbContext>();
            mockDbContext.Setup(context => context.Set<UserProfile>()).Returns(mockDbSet.Object);

            var userProfileRepository = new GenericRepository<UserProfile>(mockDbContext.Object);
            var actualUserProfiles = userProfileRepository.GetAllAsync();
           Assert.NotNull(actualUserProfiles);
        }
    }
}
