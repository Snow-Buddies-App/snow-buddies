using System.Security.Claims;

namespace SnowBuddies.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// The id of the signed-in user, taken from the JWT's NameIdentifier claim.
        /// </summary>
        public static Guid? GetUserId(this ClaimsPrincipal principal)
        {
            var value = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(value, out var userId) ? userId : null;
        }
    }
}
