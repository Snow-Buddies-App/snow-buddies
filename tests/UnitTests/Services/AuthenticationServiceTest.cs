using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SnowBuddies.Application.Implementation.Services;
using SnowBuddies.Application.Settings;
using SnowBuddies.Domain.Entities;

namespace UnitTests.Services
{
    public class AuthenticationServiceTest
    {
        private static readonly JwtSettings Settings = new()
        {
            SigningKey = "test-signing-key-that-is-long-enough-for-hs256",
            Issuer = "snow-buddies-api",
            Audience = "snow-buddies-app",
            ExpiryMinutes = 30
        };

        [Fact]
        public void CreateToken_ShouldProduceValidTokenWithUserClaims()
        {
            var user = new User { UserId = Guid.NewGuid(), DisplayName = "Mystery", Email = "mystery@example.com" };

            var result = new AuthenticationService(Settings).CreateToken(user);

            var principal = new JwtSecurityTokenHandler { MapInboundClaims = true }.ValidateToken(result.Token, new TokenValidationParameters
            {
                IssuerSigningKey = Settings.GetSecurityKey(),
                ValidIssuer = Settings.Issuer,
                ValidAudience = Settings.Audience
            }, out _);

            Assert.Equal(user.UserId.ToString(), principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Assert.Equal(user.Email, principal.FindFirst(ClaimTypes.Email)?.Value);
            Assert.Equal(user.UserId, result.UserId);
            Assert.InRange(result.ExpiresAtUtc, DateTime.UtcNow.AddMinutes(29), DateTime.UtcNow.AddMinutes(31));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("too-short")]
        public void JwtSettings_MissingOrShortKey_Throws(string? key)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["AppSettings:Token"] = key })
                .Build();

            Assert.Throws<InvalidOperationException>(() => JwtSettings.FromConfiguration(configuration));
        }

        [Fact]
        public void GoogleAuthSettings_ReadsCommaSeparatedAndLegacyClientIds()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Authentication:Google:ClientIds"] = "ios-client, android-client",
                    ["Authentication:Google:ClientId"] = "web-client"
                })
                .Build();

            var settings = GoogleAuthSettings.FromConfiguration(configuration);

            Assert.Equal(new[] { "ios-client", "android-client", "web-client" }, settings.ClientIds);
        }
    }
}
