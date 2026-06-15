using CinemaMS.Application.Features.Users.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Identity;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Users.Commands;

public class UpdateUserCommandHandler : CommandHandlerBase<UpdateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        if (user == null)
            throw new NotFoundException(nameof(User), request.Id);

        // Check if email is being changed and if it already exists
        if (user.Email != request.Email)
        {
            if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
                throw new UserFriendlyException("Email already exists and is taken by another user.");
        }

        // Validate Role
        if (user.RoleId != request.RoleId)
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
            if (role == null)
                throw new NotFoundException(nameof(Role), request.RoleId);
            user.Role = role;
            user.RoleId = request.RoleId;
        }

        user.Email = request.Email;
        user.FullName = request.FullName;
        user.PhoneNumber = request.PhoneNumber;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return UserDto.FromEntity(user);
    }
}
