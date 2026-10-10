using System.ComponentModel.DataAnnotations;

namespace SnowBuddies.Api.Models
{
    public class GoogleSignInModel
    {
        /// <summary>
        /// The ID token returned by Google Sign-In on the client.
        /// </summary>
        [Required]
        public string? IdToken { get; set; }
    }
}
