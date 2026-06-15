using CinemaMS.Application.Features.Users.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Users.Commands;

public class UpdateUserCommand : CommandBase<UserDto>
{
    public int Id { get; set; }
    public string Email { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public int RoleId { get; set; }
}
