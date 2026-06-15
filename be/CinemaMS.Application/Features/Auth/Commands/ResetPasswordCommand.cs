using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Auth.Commands;

public class ResetPasswordCommand : CommandBase<bool>
{
    public string Email { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
