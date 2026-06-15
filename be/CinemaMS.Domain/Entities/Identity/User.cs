using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;

namespace CinemaMS.Domain.Entities.Identity;

[Table("Users")]
public class User : EntityBase
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = default!;

    [Required]
    [MaxLength(500)]
    public string PasswordHash { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = default!;

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = default!;

    [MaxLength(20)]
    public string PhoneNumber { get; set; } = default!;

    public int RoleId { get; set; }

    // Fk
    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; } = default!;
}
