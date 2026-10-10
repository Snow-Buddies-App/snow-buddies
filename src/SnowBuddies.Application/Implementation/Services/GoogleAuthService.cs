using Google.Apis.Auth;
using SnowBuddies.Application.Dtos;
using SnowBuddies.Application.Interfaces.IServices;
using SnowBuddies.Application.Settings;
using SnowBuddies.Domain.Exceptions;

namespace SnowBuddies.Application.Implementation.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly GoogleAuthSettings _settings;

        public GoogleAuthService(GoogleAuthSettings settings)
        {
            _settings = settings;
        }

        public async Task<GoogleUserInfo> ValidateIdTokenAsync(string idToken)
        {
            if (_settings.ClientIds.Count == 0)
            {
                throw new InvalidOperationException("Google sign-in is not configured. Set Authentication:Google:ClientIds.");
            }

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = _settings.ClientIds
                });
            }
            catch (InvalidJwtException)
            {
                throw new UnauthorizedException("Invalid Google token");
            }

            return new GoogleUserInfo
            {
                Email = payload.Email,
                EmailVerified = payload.EmailVerified,
                GivenName = payload.GivenName,
                FamilyName = payload.FamilyName,
                Picture = payload.Picture
            };
        }
    }
}
