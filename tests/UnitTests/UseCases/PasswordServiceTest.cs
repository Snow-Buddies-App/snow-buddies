using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SnowBuddies.Application.Implementation.Services;
using SnowBuddies.Application.Interfaces.IServices;

namespace UnitTests.UseCases
{
    public class PasswordServiceTest
    {        
        [Fact]
        public void CreatePasswordHash_ValidPassword_ReturnsNonNullHashAndSalt()
        {
            string password = "MeatBalls";
            byte[] expectedPasswordHash;
            byte[] expectedPasswordSalt;


            var passwordService = new PasswordService();
            passwordService.CreatePasswordHash(password, out expectedPasswordHash, out expectedPasswordSalt);

            Assert.NotNull(expectedPasswordSalt);
            Assert.NotNull(expectedPasswordHash);
        }

        [Fact]
        public void CreatePasswordHash_EmptyPassword_ThrowsException()
        {
            string password = string.Empty;
            byte[] expectedPasswordHash;
            byte[] expectedPasswordSalt;

            var passwordService = new PasswordService();

            Assert.Throws<ArgumentNullException>(() => passwordService.CreatePasswordHash(password, out expectedPasswordHash, out expectedPasswordSalt));
        }

        [Fact]
        public void CreatePasswordHash_NullPassword_ThrowsException()
        {
            string password = null!;
            byte[] expectedPasswordHash;
            byte[] expectedPasswordSalt;

            var passwordService = new PasswordService();

            Assert.Throws<ArgumentNullException>(() => passwordService.CreatePasswordHash(password, out expectedPasswordHash, out expectedPasswordSalt));
        }
    }
}


