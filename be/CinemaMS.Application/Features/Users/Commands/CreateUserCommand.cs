using CinemaMS.Application.Features.Users.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Users.Commands;

public class CreateUserCommand : CommandBase<UserDto>
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public int RoleId { get; set; }
}
