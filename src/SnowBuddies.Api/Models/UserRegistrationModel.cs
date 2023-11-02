using System.ComponentModel.DataAnnotations;

namespace SnowBuddies.Api.Models
{
    public class UserRegistrationModel
    {
        [Required]
        [MaxLength(30)]
        [Display(Name = "Display Name")]
        public string? DisplayName { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(30)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
