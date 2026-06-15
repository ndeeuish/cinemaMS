using CinemaMS.Application.Features.Auth.DTOs;
using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Auth.Commands;

public class LoginCommand : CommandBase<AuthResponseDto>
{
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}
