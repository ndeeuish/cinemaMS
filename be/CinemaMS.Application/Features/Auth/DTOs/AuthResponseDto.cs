namespace CinemaMS.Application.Features.Auth.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string Role { get; set; } = default!;
}
