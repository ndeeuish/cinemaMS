using CinemaMS.Application.Features.Auth.DTOs;
using CinemaMS.Application.Interfaces.Security;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Auth.Commands;

public class LoginCommandHandler : CommandHandlerBase<LoginCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public override async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        
        if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UserFriendlyException("Invalid username or password");
        }

        var token = _jwtProvider.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role?.Name ?? "Customer"
        };
    }
}
