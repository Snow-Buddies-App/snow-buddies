using SnowBuddies.Application.Dtos;

namespace SnowBuddies.Application.Interfaces.IServices
{
    public interface IGoogleAuthService
    {
        /// <summary>
        /// Validates a Google ID token's signature, expiry and audience. Throws UnauthorizedException if invalid.
        /// </summary>
        Task<GoogleUserInfo> ValidateIdTokenAsync(string idToken);
    }
}
