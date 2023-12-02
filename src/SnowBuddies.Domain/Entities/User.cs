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

    [Required]
    public byte[] PasswordHash { get; set; } = new byte[32];

    [Required]
    public byte[] PasswordSalt { get; set; } = new byte[32];

    public virtual UserProfile? UserProfile { get; set; } = null!;
}
