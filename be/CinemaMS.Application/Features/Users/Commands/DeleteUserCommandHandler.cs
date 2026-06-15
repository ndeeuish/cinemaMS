using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Identity;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Users.Commands;

public class DeleteUserCommandHandler : CommandHandlerBase<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
            throw new NotFoundException(nameof(User), request.Id);

        // This will trigger the soft delete logic in ApplicationDbContext
        _userRepository.Delete(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
