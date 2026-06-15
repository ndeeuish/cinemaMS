using CinemaMS.Application.Messaging;

namespace CinemaMS.Application.Features.Auth.Commands;

public class ForgotPasswordCommand : CommandBase<bool>
{
    public string Email { get; set; } = string.Empty;
}
