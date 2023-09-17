using System.ComponentModel.DataAnnotations;
using SnowBuddies.Domain.Entities.Enums;

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
    [Display(Name = "First Name")]
    [MaxLength(30)]
    public string? FirstName { get; set; }
    [Required]
    [Display(Name = "Last Name")]
    [MaxLength(30)]
    public string? LastName { get; set; }
    public AccountStatus AccountStatus { get; set; }
    public virtual UserProfile UserProfile { get; set; } = null!;

}
