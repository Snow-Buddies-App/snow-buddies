using Npgsql;

namespace SnowBuddies.Infrastructure.Data;

public static class PostgresConnectionString
{
    /// <summary>
    /// Builds an Npgsql connection string. Accepts either a key/value connection string
    /// ("Host=...;Database=...") or a URL ("postgres://user:pass@host:5432/db"), which is the
    /// format hosting providers such as Render, Railway and Heroku hand out.
    /// An explicit <paramref name="passwordOverride"/> replaces any password in the string.
    /// </summary>
    public static string Build(string? connectionString, string? passwordOverride = null)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Database connection string is missing. Set 'ConnectionStrings:SBConnectionString' (env var ConnectionStrings__SBConnectionString) or DATABASE_URL.");
        }

        var builder = IsUrl(connectionString)
            ? FromUrl(connectionString)
            : new NpgsqlConnectionStringBuilder(connectionString);

        // GSS/Kerberos encryption isn't used by managed Postgres hosts, and probing for it logs a
        // missing libgssapi error in slim containers.
        if (!connectionString.Contains("GSS", StringComparison.OrdinalIgnoreCase))
        {
            builder.GssEncryptionMode = GssEncryptionMode.Disable;
        }

        if (!string.IsNullOrEmpty(passwordOverride))
        {
            builder.Password = passwordOverride;
        }

        return builder.ToString();
    }

    private static bool IsUrl(string value) =>
        value.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase);

    private static NpgsqlConnectionStringBuilder FromUrl(string url)
    {
        var uri = new Uri(url);
        var userInfo = uri.UserInfo.Split(':', 2);

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.IsDefaultPort || uri.Port <= 0 ? 5432 : uri.Port,
            Database = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/')),
            Username = Uri.UnescapeDataString(userInfo[0]),
        };

        if (userInfo.Length > 1)
        {
            builder.Password = Uri.UnescapeDataString(userInfo[1]);
        }

        foreach (var pair in uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = pair.Split('=', 2);
            if (parts.Length == 2 && parts[0].Equals("sslmode", StringComparison.OrdinalIgnoreCase)
                && Enum.TryParse<SslMode>(parts[1].Replace("-", string.Empty), ignoreCase: true, out var sslMode))
            {
                builder.SslMode = sslMode;
            }
        }

        return builder;
    }
}
