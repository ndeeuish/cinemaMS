using System.ComponentModel.DataAnnotations.Schema;
using CinemaMS.Domain.Common;

namespace CinemaMS.Domain.Entities.Identity;

[Table("RolePermissions")]
public class RolePermission : EntityBase
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }

    // Fk
    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; } = default!;

    [ForeignKey(nameof(PermissionId))]
    public Permission Permission { get; set; } = default!;
}
