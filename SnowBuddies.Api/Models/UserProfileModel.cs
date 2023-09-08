using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnowBuddies.Api.Models
{
    public class UserProfileModel
    {
        public Guid? ProfileId { get; set; }
        public Guid UserId { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
    }
}