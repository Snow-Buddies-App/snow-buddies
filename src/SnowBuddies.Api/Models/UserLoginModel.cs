using System.ComponentModel.DataAnnotations;

namespace SnowBuddies.Api.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Field required")]
        [EmailAddress(ErrorMessage = "Invalid EmailAddress")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
