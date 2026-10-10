using SnowBuddies.Domain.Entities;

namespace SnowBuddies.Application.Interfaces.IServices
{
    public interface IExternalLoginService
    {
        /// <summary>
        /// Validates a Google ID token and returns the matching user, creating one on first sign-in.
        /// </summary>
        Task<User> LoginWithGoogleAsync(string idToken);
    }
}
