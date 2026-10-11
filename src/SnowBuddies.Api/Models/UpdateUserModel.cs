using System.ComponentModel.DataAnnotations;

namespace SnowBuddies.Api.Models
{
    public class UpdateUserModel
    {
        [Required(ErrorMessage = "Field required")]
        [MaxLength(30)]
        [Display(Name = "Display Name")]
        public string? DisplayName { get; set; }

        [Required(ErrorMessage = "Field required")]
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Invalid EmailAddress")]
        [Display(Name = "Email")]
        public string? Email { get; set; }
    }
}
