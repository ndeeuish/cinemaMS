using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;

namespace CinemaMS.Domain.Entities.Identity;

[Table("Permissions")]
public class Permission : EntityBase
{
    [Required]
    [MaxLength(100)]
    public string Code { get; set; } = default!;

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = default!;

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
