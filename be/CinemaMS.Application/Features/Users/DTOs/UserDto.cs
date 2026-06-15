using CinemaMS.Domain.Entities.Identity;

namespace CinemaMS.Application.Features.Users.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; } = default!;
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }

    public static UserDto FromEntity(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            RoleId = user.RoleId,
            RoleName = user.Role?.Name ?? "Unknown",
            IsDeleted = user.IsDeleted,
            CreatedAt = user.CreatedAt
        };
    }
}
