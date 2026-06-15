using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Auth.Commands;

public class RegisterCommand : CommandBase<bool>
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}
