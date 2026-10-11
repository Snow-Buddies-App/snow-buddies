namespace SnowBuddies.Api.Controllers
{
    public static class RateLimitPolicies
    {
        /// <summary>
        /// Per-IP limit for login and sign-up endpoints, to slow down password guessing.
        /// </summary>
        public const string Auth = "auth";
    }
}
