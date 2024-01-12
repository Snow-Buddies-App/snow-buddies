using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SnowBuddies.Application.Dtos
{
    public class GoogleTokenValidationData
    {
        [JsonProperty("issued_to")]
        public string? IssuedTo { get; set; }

        [JsonProperty("audience")]
        public string? Audience { get; set; }

        [JsonProperty("user_id")]
        public string? UserId { get; set; }

        [JsonProperty("scope")]
        public Uri? Scope { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("verified_email")]
        public bool VerifiedEmail { get; set; }

        [JsonProperty("access_type")]
        public string? AccessType { get; set; }

    }
}
