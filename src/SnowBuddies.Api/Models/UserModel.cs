using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SnowBuddies.Domain.Entities.Enums;

namespace SnowBuddies.Api.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Field required")]
        [MaxLength(30)]
        [Display(Name = "Display Name")]
        public string? DisplayName { get; set; }

        [Required(ErrorMessage = "Field required")]
        [EmailAddress(ErrorMessage = "Invalid EmailAddress")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Field required")]
        [Display(Name = "First Name")]
        [MaxLength(30)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Field required")]
        [Display(Name = "Last Name")]
        [MaxLength(30)]
        public string? LastName { get; set; }

        public AccountStatus AccountStatus { get; set; }
    }
}
