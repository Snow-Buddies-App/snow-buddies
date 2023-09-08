using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SnowBuddies.Domain.Entities
{
    public class UserProfile
    {
        [Key]
        public Guid? ProfileId { get; set; }
        public Guid UserId { get; set; }

        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }

        public virtual User User { get; set; } = null!;

    }
}