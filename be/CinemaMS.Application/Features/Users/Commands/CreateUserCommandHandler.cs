using CinemaMS.Application.Features.Users.DTOs;
using CinemaMS.Application.Interfaces.Security;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Identity;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Users.Commands;

public class CreateUserCommandHandler : CommandHandlerBase<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public override async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByUsernameAsync(request.Username, cancellationToken))
            throw new UserFriendlyException("Username already exists");

        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new UserFriendlyException("Email already exists");

        var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
        if (role == null)
            throw new NotFoundException(nameof(Role), request.RoleId);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            PasswordHash = _passwordHasher.Hash(request.Password),
            RoleId = request.RoleId,
            IsDeleted = false
        };

        _userRepository.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map DTO
        user.Role = role;
        return UserDto.FromEntity(user);
    }
}
