using CinemaMS.Domain.Entities.Identity;

namespace CinemaMS.Application.Interfaces.Security;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
