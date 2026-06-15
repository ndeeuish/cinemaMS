using CinemaMS.Application.Interfaces.Security;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Identity;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Auth.Commands;

public class RegisterCommandHandler : CommandHandlerBase<RegisterCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(
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

    public override async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByUsernameAsync(request.Username, cancellationToken))
            throw new UserFriendlyException("Username already exists");

        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new UserFriendlyException("Email already exists");

        var customerRole = await _roleRepository.GetByNameAsync("Customer", cancellationToken);
        if (customerRole == null)
            throw new Exception("Default Customer role not found in database.");

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            PasswordHash = _passwordHasher.Hash(request.Password),
            RoleId = customerRole.Id,
            IsDeleted = false
        };

        _userRepository.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
