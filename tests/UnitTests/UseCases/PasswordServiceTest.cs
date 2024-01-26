using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private readonly IPasswordService _passwordService;
        public PasswordServiceTest()
        {
            _passwordService = new PasswordService();
        }

        [Fact]
        public void CreatePasswordHash_ValidPassword_ReturnsNonNullHashAndSalt()
        {
            string password = "MeatBalls";
            byte[] expectedPasswordHash;
            byte[] expectedPasswordSalt;

            _passwordService.CreatePasswordHash(password, out expectedPasswordHash, out expectedPasswordSalt);

            Assert.NotNull(expectedPasswordSalt);
            Assert.NotNull(expectedPasswordHash);
        }

        [Fact]
        public void CreatePasswordHash_EmptyPassword_ThrowsException()
        {
            string password = string.Empty;
            byte[] expectedPasswordHash;
            byte[] expectedPasswordSalt;

            Assert.Throws<ArgumentNullException>(() => _passwordService.CreatePasswordHash(password, out expectedPasswordHash, out expectedPasswordSalt));
        }

        [Fact]
        public void CreatePasswordHash_NullPassword_ThrowsException()
        {
            string password = null!;
            byte[] expectedPasswordHash;
            byte[] expectedPasswordSalt;

            Assert.Throws<ArgumentNullException>(() => _passwordService.CreatePasswordHash(password, out expectedPasswordHash, out expectedPasswordSalt));
        }

        [Fact]
        public void VerifyPassword_InvalidPassword_ShouldReturnFalse()
        {
            var password = "Password123";
            var passwordToBeVerified = "IncorrectPassword456";

            _passwordService.CreatePasswordHash(password, out var storedPasswordHash, out var storedPasswordSalt);
            var isPasswordValid = _passwordService.VerifyPasswordHash(passwordToBeVerified, storedPasswordHash, storedPasswordSalt);

            Assert.False(isPasswordValid);
        }

        [Fact]
        public void VerifyPassword_ValidPassword_ShouldReturnTrue() 
        {
            var password = "Password123";
            var passwordToBeVerified = "Password123";

            _passwordService.CreatePasswordHash(password, out var storedPasswordHash, out var storedPasswordSalt);
            var isPasswordValid = _passwordService.VerifyPasswordHash(passwordToBeVerified, storedPasswordHash, storedPasswordSalt);
            
            Assert.True(isPasswordValid);
        }
    }
}
