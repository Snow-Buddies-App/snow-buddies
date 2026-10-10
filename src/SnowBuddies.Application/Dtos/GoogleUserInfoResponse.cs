using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SnowBuddies.Application.Dtos
{
    public class GoogleUserInfoResponse
    {
        [JsonProperty("family_name")]
        public string? FamilyName { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("picture")]
        public Uri? Picture { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("given_name")]
        public string? GivenName { get; set; }

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("verified_email")]
        public bool VerifiedEmail { get; set; }
    }
}
