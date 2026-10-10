using Npgsql;
using SnowBuddies.Infrastructure.Data;

namespace UnitTests.Infrastructure
{
    public class PostgresConnectionStringTest
    {
        [Fact]
        public void Build_KeyValueString_AppliesPasswordOverride()
        {
            var result = new NpgsqlConnectionStringBuilder(
                PostgresConnectionString.Build("Server=localhost;Database=SnowBuddiesDb;Username=postgres", "secret"));

            Assert.Equal("localhost", result.Host);
            Assert.Equal("SnowBuddiesDb", result.Database);
            Assert.Equal("postgres", result.Username);
            Assert.Equal("secret", result.Password);
        }

        [Fact]
        public void Build_PostgresUrl_IsConverted()
        {
            var result = new NpgsqlConnectionStringBuilder(
                PostgresConnectionString.Build("postgresql://snow_user:p%40ss%3Aword@dpg-abc123-a.oregon-postgres.render.com/snowbuddies?sslmode=require"));

            Assert.Equal("dpg-abc123-a.oregon-postgres.render.com", result.Host);
            Assert.Equal(5432, result.Port);
            Assert.Equal("snowbuddies", result.Database);
            Assert.Equal("snow_user", result.Username);
            Assert.Equal("p@ss:word", result.Password);
            Assert.Equal(SslMode.Require, result.SslMode);
        }

        [Fact]
        public void Build_PostgresUrlWithPort_UsesPort()
        {
            var result = new NpgsqlConnectionStringBuilder(
                PostgresConnectionString.Build("postgres://user:pw@db.internal:6543/app"));

            Assert.Equal(6543, result.Port);
            Assert.Equal("pw", result.Password);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Build_Missing_Throws(string? connectionString)
        {
            Assert.Throws<InvalidOperationException>(() => PostgresConnectionString.Build(connectionString));
        }
    }
}
