using System.Security.Cryptography;
using System.Text;
using SnowBuddies.Application.Interfaces.IServices;

namespace SnowBuddies.Application.Implementation.Services
{
    public class PasswordService : IPasswordService
    {
        // PBKDF2-HMAC-SHA512 with the OWASP recommended iteration count.
        private const int Iterations = 210_000;
        private const int SaltSize = 16;
        private const int HashSize = 64;

        // Hashes created before PBKDF2 used a single HMAC-SHA512 keyed with a 128 byte salt.
        private const int LegacySaltSize = 128;

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            ValidateNonEmptyPassword(password);

            passwordSalt = RandomNumberGenerator.GetBytes(SaltSize);
            passwordHash = Rfc2898DeriveBytes.Pbkdf2(password, passwordSalt, Iterations, HashAlgorithmName.SHA512, HashSize);
        }

        public bool VerifyPasswordHash(string password, byte[]? storedPasswordHash, byte[]? storedPasswordSalt)
        {
            ValidateNonEmptyPassword(password);

            if (storedPasswordHash == null || storedPasswordSalt == null)
            {
                return false;
            }

            byte[] computedHash;
            if (storedPasswordSalt.Length == LegacySaltSize)
            {
                using var hmac = new HMACSHA512(storedPasswordSalt);
                computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
            else
            {
                computedHash = Rfc2898DeriveBytes.Pbkdf2(password, storedPasswordSalt, Iterations, HashAlgorithmName.SHA512, HashSize);
            }

            return CryptographicOperations.FixedTimeEquals(computedHash, storedPasswordHash);
        }

        private static void ValidateNonEmptyPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }
        }
    }
}
