namespace CinemaMS.Application.Interfaces.Security;

public interface ICurrentUserService
{
    string? UserId { get; }
}
