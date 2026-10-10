using Microsoft.Extensions.Configuration;

namespace SnowBuddies.Application.Settings
{
    public class GoogleAuthSettings
    {
        /// <summary>
        /// OAuth client IDs whose ID tokens are accepted (web, iOS and Android apps each have their own).
        /// </summary>
        public IReadOnlyList<string> ClientIds { get; init; } = Array.Empty<string>();

        /// <summary>
        /// Reads "Authentication:Google:ClientIds" (comma separated) and the legacy single
        /// "Authentication:Google:ClientId".
        /// </summary>
        public static GoogleAuthSettings FromConfiguration(IConfiguration configuration)
        {
            var ids = new List<string>();
            foreach (var key in new[] { "Authentication:Google:ClientIds", "Authentication:Google:ClientId" })
            {
                var value = configuration[key];
                if (!string.IsNullOrWhiteSpace(value))
                {
                    ids.AddRange(value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
                }
            }

            return new GoogleAuthSettings { ClientIds = ids.Distinct().ToList() };
        }
    }
}
