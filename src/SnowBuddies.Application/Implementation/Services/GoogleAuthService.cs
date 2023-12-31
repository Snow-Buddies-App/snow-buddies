using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SnowBuddies.Application.Dtos;
using SnowBuddies.Application.Interfaces.IServices;

namespace SnowBuddies.Application.Implementation.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private const string TokenValidationUrl = "https://www.googleapis.com/oauth2/v2/tokeninfo?access_token={0}";
        private const string UserInfoUrl = "https://www.googleapis.com/userinfo/v2/me?fields=family_name,email,name,given_name,family_name,picture&access_token={0}";
        private readonly IHttpClientFactory _httpClientFactory;

        public GoogleAuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GoogleTokenValidationData> ValidateAccessTokenAsync(string accessToken)
        {
            var formattedUrl = string.Format(TokenValidationUrl, accessToken);
            var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
            result.EnsureSuccessStatusCode();
            var responseAsString = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GoogleTokenValidationData>(responseAsString)!;
        }

        public async Task<GoogleUserInfoResponse> GetUserInfoAsync(string accessToken)
        {
            var formattedUrl = string.Format(UserInfoUrl, accessToken);
            var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
            result.EnsureSuccessStatusCode();
            var responseAsString = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GoogleUserInfoResponse>(responseAsString)!;
        }
    }
}
