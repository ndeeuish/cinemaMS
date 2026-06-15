using CinemaMS.Application.Features.Users.DTOs;
using CinemaMS.Application.Messaging;
using CinemaMS.Domain.Entities.Identity;
using CinemaMS.Domain.Exceptions;
using CinemaMS.Domain.Repositories;

namespace CinemaMS.Application.Features.Users.Queries;

public class GetUserByIdQueryHandler : QueryHandlerBase<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        if (user == null)
            throw new NotFoundException(nameof(User), request.Id);

        return UserDto.FromEntity(user);
    }
}
