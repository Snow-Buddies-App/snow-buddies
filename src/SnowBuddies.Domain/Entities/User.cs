using System.ComponentModel.DataAnnotations;

namespace SnowBuddies.Domain.Entities;

public class User
{
    [Key]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(30)]
    [Display(Name = "Display Name")]
    public string? DisplayName { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    // Null for accounts created through an external provider (e.g. Google) that have no password.
    public byte[]? PasswordHash { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public virtual UserProfile? UserProfile { get; set; }
}
