using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using SnowBuddies.Domain.Entities.Enums;

namespace SnowBuddies.Domain.Entities
{
    public class UserProfile
    {
        [Key]
        public Guid UserProfileId { get; set; }

        public Guid UserId { get; set; }
        public string? ProfilePicture { get; set; }

        public string? Bio { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(30)]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(30)]
        public string? LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public AccountStatus AccountStatus { get; set; }
        public virtual User? User { get; set; }

    }
}
