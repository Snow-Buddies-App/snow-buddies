using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace SnowBuddies.Application.Settings
{
    public class JwtSettings
    {
        // HMAC-SHA256 needs a key of at least 256 bits.
        public const int MinimumKeyLengthBytes = 32;

        public string SigningKey { get; init; } = string.Empty;
        public string? Issuer { get; init; }
        public string? Audience { get; init; }
        public int ExpiryMinutes { get; init; } = 60;

        public SymmetricSecurityKey GetSecurityKey() => new(Encoding.UTF8.GetBytes(SigningKey));

        /// <summary>
        /// Reads JWT settings from configuration. The signing key lives at "AppSettings:Token"
        /// (env var AppSettings__Token); the rest under the "Jwt" section.
        /// </summary>
        public static JwtSettings FromConfiguration(IConfiguration configuration)
        {
            var signingKey = configuration["AppSettings:Token"];
            if (string.IsNullOrWhiteSpace(signingKey) || Encoding.UTF8.GetByteCount(signingKey) < MinimumKeyLengthBytes)
            {
                throw new InvalidOperationException(
                    $"JWT signing key is missing or too short. Set 'AppSettings:Token' (env var AppSettings__Token) to a random secret of at least {MinimumKeyLengthBytes} characters.");
            }

            return new JwtSettings
            {
                SigningKey = signingKey,
                Issuer = NullIfEmpty(configuration["Jwt:Issuer"]),
                Audience = NullIfEmpty(configuration["Jwt:Audience"]),
                ExpiryMinutes = int.TryParse(configuration["Jwt:ExpiryMinutes"], out var minutes) && minutes > 0 ? minutes : 60
            };
        }

        private static string? NullIfEmpty(string? value) => string.IsNullOrWhiteSpace(value) ? null : value;
    }
}
